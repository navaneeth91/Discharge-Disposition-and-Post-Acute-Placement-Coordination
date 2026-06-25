using DischargeDisposition_Backend.Insurance.DTOs.Responses;
namespace DischargeDisposition_Backend.Insurance.Repositories.Interfaces
{
    public interface IInsuranceDashboardRepository
    {
        Task<InsuranceDashboard>GetInsuranceDashboardAsync();
        Task<List<InsuranceAnalytics>> GetServiceAnalyticsAsync();
        Task<List<AuthorizationRequestListItemResponse>> GetRecentAuthorizationRequestsAsync(int take);
    }
}
