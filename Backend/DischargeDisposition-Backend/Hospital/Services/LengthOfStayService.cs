using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
namespace DischargeDisposition_Backend.Hospital.Services
{
    public class LengthOfStayService : ILengthOfStayService
    {
        private readonly ILengthOfStayRepository _repository;

        public LengthOfStayService(ILengthOfStayRepository repository)
        {
            _repository = repository;
        }

        public async Task<LOSResponseDto?> GetPatientLOSAsync(int patientId)
        {
            var los = await _repository.GetLosByPatientIdAsync(patientId);

            if (los == null)
                throw new Exception("No records to show");

            return new LOSResponseDto
            {
                PatientId = los.PatientId,
                PatientName = $"{los.patient.FirstName} {los.patient.LastName}",
                AdmissionDate = los.patient.AdmissionDate,
                LastCalculatedDate = los.LastCalculatedDate,
                VarianceDays = los.VarianceDays
            };
        }

        public async Task<List<LOSResponseDto>> GetAllLOSAsync()
        {
            var records = await _repository.GetAllLosAsync();
            if (records == null)
            {
                throw new Exception("No available patient records");
            }
            return records.Select(los => new LOSResponseDto
            {
                PatientId = los.PatientId,
                PatientName = $"{los.patient.FirstName} {los.patient.LastName}",
                AdmissionDate = los.patient.AdmissionDate,
                LastCalculatedDate = los.LastCalculatedDate,
                VarianceDays = los.VarianceDays
            }).ToList();


        }

        
    }
}
