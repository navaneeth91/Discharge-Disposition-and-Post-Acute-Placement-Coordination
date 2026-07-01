using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class DispositionDecisionRepository
        : IDispositionDecisionRepository
    {
        private readonly HospitalDbContext _context;

        public DispositionDecisionRepository(
            HospitalDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            DispositionDecision decision)
        {
            await _context.DispositionDecisions
                .AddAsync(decision);

            await _context.SaveChangesAsync();
        }
        public async Task<DispositionDecision?> GetByPatientIdAsync(
    int patientId)
        {
            return await _context.DispositionDecisions
                .AsNoTracking()
                .Include(x => x.patient)
                .Include(x => x.dispositionType)
                .Include(x => x.clinician)
                .Include(x => x.department)
                .OrderByDescending(x => x.DecisionDate)
                .FirstOrDefaultAsync(x => x.PatientId == patientId);
        }

        public async Task<DispositionDecision?> GetByPatientIdWithTrackingAsync(
            int patientId)
        {
            return await _context.DispositionDecisions

                .FirstOrDefaultAsync(x =>
                    x.PatientId == patientId);
        }

        public async Task<DispositionDecision?> GetByDecisionIdAsync(
    int decisionId)
        {
            return await _context.DispositionDecisions
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.DecisionId == decisionId);
        }

        public async Task UpdateDecisionAsync(
            DispositionDecision decision)
        {
            _context.DispositionDecisions.Update(decision);

            await _context.SaveChangesAsync();
        }

        public async Task<List<AssignedPatientsResponse>> GetAssignedPatientsAsync(int clinicianId, string? search)
        {
            var query = _context.DispositionDecisions
                .Include(d => d.patient)
                .Include(d => d.dispositionType)
                .Where(d => d.ClinicianId == clinicianId);
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(d =>

                    d.patient.FirstName.ToLower().Contains(search)

                    ||

                    d.patient.LastName.ToLower().Contains(search)

                    ||

                    (d.patient.FirstName + " " + d.patient.LastName)
                        .ToLower()
                        .Contains(search)

                );
            }

            return await _context.DispositionDecisions

                .Include(d => d.patient)
                .Include(d => d.dispositionType)

                .Where(d => d.ClinicianId == clinicianId)

                .Select(d => new AssignedPatientsResponse
                {
                    PatientId = d.PatientId,

                    PatientName =
                        d.patient.FirstName + " " +
                        d.patient.LastName,

                    DecisionDate =
                        d.DecisionDate,

                    Status =
                        d.Status.ToString(),

                    Disposition =
                        d.dispositionType.DispositionName
                })

                .ToListAsync();
        }
    }
}