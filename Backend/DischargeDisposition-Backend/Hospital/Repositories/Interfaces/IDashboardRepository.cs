using DischargeDisposition_Backend.Hospital.DTOs.Responses;
namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<HospitalDashboard>GetHospitalDashboardAsync();
        Task<List<PatientDistribution>>GetPatientDistributionAsync();
        Task<List<AuthorizationAnalytics>>GetAuthorizationAnalyticsAsync();
    }
}
