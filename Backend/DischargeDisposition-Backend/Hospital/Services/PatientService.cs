using DischargeDisposition_Backend.Hospital.DTOs.Requests;
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
        public PatientResponseDto? GetPatientById(int patientId)
        {
            var patient = _repository.GetPatientById(patientId);

            if (patient == null)
                return null;

            return new PatientResponseDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                AdmissionDate = patient.AdmissionDate,
                IsActive = patient.IsActive
            };
        }
        public async Task<bool> UpdatePatientAsync(
    int patientId,
    UpdateUserDto dto)
        {
            var patient =
                _repository.GetPatientById(patientId);

            if (patient == null)
                return false;

            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;

            return await _repository
                .UpdatePatientAsync(patient);
        }
    }
}
