using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class DispositionDecisionService : IDispositionDecisionService
    {
        private readonly HospitalDbContext _db;
        private readonly ILogger<DispositionDecisionService> _logger;

        public DispositionDecisionService(HospitalDbContext db, ILogger<DispositionDecisionService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DispositionDecisionResponseDto?> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            try
            {
                var decision = await _db.DispositionDecisions
                    .Where(d => d.PatientId == patientId)
                    .Include(d => d.dispositionType)
                    .FirstOrDefaultAsync(cancellationToken);

                return decision is null ? null : MapToResponse(decision);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching disposition decision for patient {PatientId}", patientId);
                throw;
            }
        }

        public async Task<DispositionDecisionResponseDto> UpdateAsync(int decisionId, UpdateDispositionDecisionDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var decision = await _db.DispositionDecisions
                    .Include(d => d.dispositionType)
                    .FirstOrDefaultAsync(d => d.DecisionId == decisionId, cancellationToken);

                if (decision is null)
                    throw new KeyNotFoundException($"Disposition decision with id {decisionId} not found");

                // Validate disposition type exists
                var dispositionTypeExists = await _db.DispositionTypes
                    .AnyAsync(dt => dt.DispositionTypeId == dto.DispositionTypeId, cancellationToken);

                if (!dispositionTypeExists)
                    throw new KeyNotFoundException($"Disposition type with id {dto.DispositionTypeId} not found");

                // Validate provider exists
                var providerExists = await _db.PostAcuteProviders
                    .AnyAsync(p => p.ProviderId == dto.ProviderId, cancellationToken);

                if (!providerExists)
                    throw new KeyNotFoundException($"Provider with id {dto.ProviderId} not found");

                // Update fields
                decision.DispositionTypeId = dto.DispositionTypeId;
                decision.Notes = dto.Notes;
                decision.ExpectedTransitionDate = DateOnly.FromDateTime(dto.PlannedDischargeDate);

                _db.DispositionDecisions.Update(decision);
                await _db.SaveChangesAsync(cancellationToken);

                decision = await _db.DispositionDecisions
                    .Include(d => d.dispositionType)
                    .FirstOrDefaultAsync(d => d.DecisionId == decisionId, cancellationToken);

                return MapToResponse(decision!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating disposition decision with id {DecisionId}", decisionId);
                throw;
            }
        }

        private static DispositionDecisionResponseDto MapToResponse(Hospital.Models.DispositionDecision decision)
        {
            return new DispositionDecisionResponseDto
            {
                DecisionId = decision.DecisionId,
                PatientId = decision.PatientId,
                DispositionTypeId = decision.DispositionTypeId,
                DispositionTypeName = decision.dispositionType?.Name ?? string.Empty,
                ProviderId = 0, // Update based on your actual schema if provider is available
                ProviderName = string.Empty, // Update based on your actual schema
                Notes = decision.Notes,
                PlannedDischargeDate = decision.ExpectedTransitionDate.ToDateTime(TimeOnly.MinValue),
                CreatedDate = decision.DecisionDate,
                Status = decision.Status.ToString()
            };
        }
    }
}