using DischargeDisposition_Backend.Hospital.DTOs.Responses;
namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<HospitalDashboard>GetHospitalDashboardAsync();
        Task<List<PatientDistribution>>GetPatientDistributionAsync();

        Task<List<AuthorizationAnalytics>>GetAuthorizationAnalyticsAsync();
    }
}
