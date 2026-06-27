using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface ICareManagerRepository
    {
        Task<CareManagerDashboardResponse> GetDashboardAsync(int careManagerId);
    }
}