using DischargeDisposition_Backend.Insurance.DTOs.Responses;

namespace DischargeDisposition_Backend.Insurance.Services.Interfaces
{
    public interface IInsuranceDashboardService
    {
        Task<InsuranceDashboard>GetInsuranceDashboardAsync();

        Task<List<InsuranceAnalytics>>GetServiceAnalyticsAsync();
    }
}