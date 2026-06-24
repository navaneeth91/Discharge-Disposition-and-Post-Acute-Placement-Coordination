using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class PatientRepository
        : IPatientRepository
    {
        private readonly
            ILogger<PatientRepository>
            _logger;

        private readonly
            HospitalDbContext
            _context;

        public PatientRepository(
            ILogger<PatientRepository> logger,
            HospitalDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<Patient>>
            GetPatientsAsync()
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving patient data.");

                return await _context.Patients
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to retrieve patients.");

                throw;
            }
        }

        public async Task<Patient?>
            GetByIdAsync(int patientId)
        {
            return await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    p => p.PatientId == patientId);
        }

        public async Task<Patient?>
            GetTrackedByIdAsync(
                int patientId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(
                    p => p.PatientId == patientId);
        }

        public async Task UpdatePatientAsync(
            Patient patient)
        {
            await _context.SaveChangesAsync();
        }
    }
}