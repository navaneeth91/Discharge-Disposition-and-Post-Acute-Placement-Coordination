using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;

namespace DischargeDisposition_Backend.Insurance.Services.Interfaces
{
    public interface IInsuranceDashboardService
    {
        Task<ApiResponse<InsuranceDashboard>>
            GetInsuranceDashboardAsync();

        Task<ApiResponse<List<InsuranceAnalytics>>>
            GetServiceAnalyticsAsync();
    }
}