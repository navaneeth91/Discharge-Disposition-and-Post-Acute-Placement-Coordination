using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class PatientService : IPatientService
    {
        private readonly ILogger<PatientService> _logger;
        private readonly IPatientRepository _repository;

        public PatientService(ILogger<PatientService> logger, IPatientRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<PatientResponseDto> GetPatients()
        {
            var patients = _repository.GetPatients();

            return patients.Select(p => new PatientResponseDto
            {
                PatientId = p.PatientId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                AdmissionDate = p.AdmissionDate,
                IsActive = p.IsActive,
            });
        }
    }
}
