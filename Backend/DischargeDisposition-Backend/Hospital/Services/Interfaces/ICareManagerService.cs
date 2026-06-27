using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface ICareManagerService
    {
        Task<ApiResponse<CareManagerDashboardResponse>> GetDashboardAsync(int careManagerId);
    }
}