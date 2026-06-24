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

        public PatientService(
            ILogger<PatientService> logger,
            IPatientRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<IEnumerable<PatientResponseDto>> GetPatientsAsync()
        {
            var patients = await _repository.GetPatientsAsync();

            return patients.Select(p => new PatientResponseDto
            {
                PatientId = p.PatientId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                AdmissionDate = p.AdmissionDate,
                IsActive = p.IsActive
            });
        }

        public async Task<PatientResponseDto?> GetPatientByIdAsync(int patientId)
        {
            var patient = await _repository.GetByIdAsync(patientId);

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

        public async Task<bool> DischargePatientAsync(
            int patientId,
            DateTime actualDischargeDate)
        {
            var patient = await _repository.GetByIdAsync(patientId);

            if (patient == null)
                return false;

            if (actualDischargeDate < patient.AdmissionDate)
                throw new ArgumentException(
                    "Actual discharge date cannot be earlier than admission date.");

            patient.ActualDischargeDate = actualDischargeDate;
            patient.IsActive = 0;

            await _repository.UpdatePatientAsync(patient);

            return true;
        }
    }
}