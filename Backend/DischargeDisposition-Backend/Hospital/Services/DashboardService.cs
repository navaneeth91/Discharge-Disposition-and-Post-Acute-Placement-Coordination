using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class DashboardService
    : IDashboardService
    {
        private readonly IDashboardRepository
            _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<HospitalDashboard>GetHospitalDashboardAsync()
        {
            return await _repository
                .GetHospitalDashboardAsync();
        }
        public async Task<List<PatientDistribution>>GetPatientDistributionAsync()
        {
            return await _repository
                .GetPatientDistributionAsync();
        }

        public async Task<List<AuthorizationAnalytics>>GetAuthorizationAnalyticsAsync()
        {
            return await _repository
                .GetAuthorizationAnalyticsAsync();
        }
    }
}
