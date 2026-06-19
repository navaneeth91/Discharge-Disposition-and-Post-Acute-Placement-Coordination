using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class ReferralService : IReferralService
    {
        private readonly IReferralRepository _repo;
        private readonly HospitalDbContext _db;

        public ReferralService(IReferralRepository repo, HospitalDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<IEnumerable<ReferralResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var items = await _repo.GetAllAsync(cancellationToken);
            return items.Select(MapToResponse).ToList();
        }

        public async Task<ReferralResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repo.GetByIdAsync(id, cancellationToken);
            return entity is null ? null : MapToResponse(entity);
        }

        public async Task<ReferralResponseDto> CreateAsync(CreateReferralDto dto, CancellationToken cancellationToken = default)
        {
            // Business validations:
            await ValidateRelationsExist(dto.PatientId, dto.ProviderId, dto.CareManagerId, cancellationToken);

            if (dto.CreatedDate.HasValue && dto.CreatedDate.Value > DateTime.UtcNow)
                throw new ArgumentException("CreatedDate cannot be in the future.");

            var entity = new Referral
            {
                PatientId = dto.PatientId,
                ProviderId = dto.ProviderId,
                CareManagerId = dto.CareManagerId,
                CreatedDate = dto.CreatedDate ?? DateTime.UtcNow,
                Status = dto.Status,
                Priority = dto.Priority
            };

            var created = await _repo.CreateAsync(entity, cancellationToken);
            return MapToResponse(created);
        }

        public async Task<bool> UpdateAsync(int id, UpdateReferralDto dto, CancellationToken cancellationToken = default)
        {
            var existing = await _repo.GetByIdAsync(id, cancellationToken);
            if (existing is null) return false;

            await ValidateRelationsExist(dto.PatientId, dto.ProviderId, dto.CareManagerId, cancellationToken);

            if (dto.CreatedDate.HasValue && dto.CreatedDate.Value > DateTime.UtcNow)
                throw new ArgumentException("CreatedDate cannot be in the future.");

            // update allowed fields
            existing.PatientId = dto.PatientId;
            existing.ProviderId = dto.ProviderId;
            existing.CareManagerId = dto.CareManagerId;
            existing.CreatedDate = dto.CreatedDate ?? existing.CreatedDate;
            existing.Status = dto.Status;
            existing.Priority = dto.Priority;

            await _repo.UpdateAsync(existing, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repo.GetByIdAsync(id, cancellationToken);
            if (entity is null) return false;

            await _repo.DeleteAsync(id, cancellationToken);
            return true;
        }

        public async Task<IEnumerable<ReferralResponseDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var items = await _repo.GetByPatientIdAsync(patientId, cancellationToken);
            return items.Select(MapToResponse).ToList();
        }

        public async Task<IEnumerable<ReferralResponseDto>> GetByProviderIdAsync(int providerId, CancellationToken cancellationToken = default)
        {
            var items = await _repo.GetByProviderIdAsync(providerId, cancellationToken);
            return items.Select(MapToResponse).ToList();
        }

        public async Task<IEnumerable<ReferralResponseDto>> GetPendingReferralsAsync(CancellationToken cancellationToken = default)
        {
            var items = await _repo.GetPendingReferralsAsync(cancellationToken);
            return items.Select(MapToResponse).ToList();
        }

        public async Task<IEnumerable<ReferralResponseDto>> GetCompletedReferralsAsync(CancellationToken cancellationToken = default)
        {
            var items = await _repo.GetCompletedReferralsAsync(cancellationToken);
            return items.Select(MapToResponse).ToList();
        }

        private ReferralResponseDto MapToResponse(Referral r)
        {
            return new ReferralResponseDto
            {
                ReferralId = r.ReferralId,
                PatientId = r.PatientId,
                ProviderId = r.ProviderId,
                CareManagerId = r.CareManagerId,
                CreatedDate = r.CreatedDate,
                Status = r.Status,
                Priority = r.Priority,
                PatientName = r.patient is null ? null : $"{r.patient.FirstName} {r.patient.LastName}",
                ProviderName = r.provider?.ProviderName,
                CareManagerName = r.careManager is null ? null : $"{r.careManager.FirstName} {r.careManager.LastName}"
            };
        }

        private async Task ValidateRelationsExist(int patientId, int providerId, int careManagerId, CancellationToken cancellationToken)
        {
            var patientExists = await _db.Patients.AnyAsync(p => p.PatientId == patientId, cancellationToken);
            if (!patientExists) throw new KeyNotFoundException($"Patient {patientId} not found.");

            var providerExists = await _db.PostAcuteProviders.AnyAsync(p => p.ProviderId == providerId, cancellationToken);
            if (!providerExists) throw new KeyNotFoundException($"Provider {providerId} not found.");

            var careManagerExists = await _db.Users.AnyAsync(u => u.UserId == careManagerId, cancellationToken);
            if (!careManagerExists) throw new KeyNotFoundException($"Care manager {careManagerId} not found.");
        }
    }
}