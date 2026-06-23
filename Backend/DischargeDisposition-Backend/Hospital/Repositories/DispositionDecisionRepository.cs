using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                .Include(x => x.patient)
                .Include(x => x.dispositionType)
                .Include(x => x.clinician)
                .Include(x => x.department)
                .OrderByDescending(x => x.DecisionDate)
                .FirstOrDefaultAsync(x => x.PatientId == patientId);
        }
        public async Task<DispositionDecision?> GetByDecisionIdAsync(
    int decisionId)
        {
            return await _context.DispositionDecisions
                .FirstOrDefaultAsync(x =>
                    x.DecisionId == decisionId);
        }

        public async Task UpdateDecisionAsync(
            DispositionDecision decision)
        {
            _context.DispositionDecisions.Update(decision);

            await _context.SaveChangesAsync();
        }
    }
}