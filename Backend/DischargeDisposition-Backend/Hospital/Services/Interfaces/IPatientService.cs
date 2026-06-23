using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IPatientService
    {
        IEnumerable<PatientResponseDto> GetPatients();
        PatientResponseDto? GetPatientById(int patientId);

        Task<bool> UpdatePatientAsync(
            int patientId,
            UpdateUserDto dto);
    }
}
