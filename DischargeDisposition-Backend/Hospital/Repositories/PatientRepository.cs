using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ILogger<PatientRepository> _logger;
        private readonly HospitalDbContext _context;

        public PatientRepository(ILogger<PatientRepository> logger, HospitalDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IEnumerable<Patient> GetPatients()
        {
            try
            {
                _logger.LogInformation("Trying to retrieve patient data from the database....");
                return _context.Patients.ToList();
            }

            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve Patient data");
            }
        }
        public Patient? GetPatientById(int patientId)
        {
            return _context.Patients
                .FirstOrDefault(x => x.PatientId == patientId);
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            _context.Patients.Update(patient);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
