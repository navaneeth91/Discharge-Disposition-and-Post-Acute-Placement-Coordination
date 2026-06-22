using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class PatientDelayRepository : IPatientDelayRepository
    {
        private readonly HospitalDbContext _context;

        public PatientDelayRepository(
            HospitalDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            PatientDelay patientDelay)
        {
            await _context.PatientDelays
                .AddAsync(patientDelay);

            await _context.SaveChangesAsync();
        }

        public async Task<List<PatientDelay>> GetByPatientIdAsync(
    int patientId)
        {
            return await _context.PatientDelays
                .Include(x => x.delayReason)
                .Include(x => x.reportedUser)
                .Where(x => x.PatientId == patientId)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }
    }
}