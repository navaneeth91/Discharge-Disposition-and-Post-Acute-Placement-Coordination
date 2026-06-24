using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<ApiResponse<HospitalDashboard>> GetHospitalDashboardAsync();

        Task<ApiResponse<List<PatientDistribution>>>GetPatientDistributionAsync();

        Task<ApiResponse<List<AuthorizationAnalytics>>>GetAuthorizationAnalyticsAsync();
    }
}