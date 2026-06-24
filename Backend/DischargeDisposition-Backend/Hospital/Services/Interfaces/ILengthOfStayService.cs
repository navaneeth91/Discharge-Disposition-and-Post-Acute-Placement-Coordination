using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface ILengthOfStayService
    {
        Task<ApiResponse<LOSResponseDto>>
            GetPatientLOSAsync(
                int patientId);

        Task<ApiResponse<List<LOSResponseDto>>>
            GetAllLOSAsync();
    }
}