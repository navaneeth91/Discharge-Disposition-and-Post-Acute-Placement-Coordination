using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class DashboardService
        : IDashboardService
    {
        private readonly IDashboardRepository
            _repository;

        public DashboardService(
            IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<HospitalDashboard>>
            GetHospitalDashboardAsync()
        {
            try
            {
                var dashboard =
                    await _repository
                        .GetHospitalDashboardAsync();

                if (dashboard == null)
                {
                    return new ApiResponse<HospitalDashboard>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "Dashboard data not found"
                    };
                }

                return new ApiResponse<HospitalDashboard>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Dashboard retrieved successfully",

                    Data = dashboard
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HospitalDashboard>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve dashboard",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<
            ApiResponse<List<PatientDistribution>>>
            GetPatientDistributionAsync()
        {
            try
            {
                var patients =
                    await _repository
                        .GetPatientDistributionAsync();

                return new ApiResponse<
                    List<PatientDistribution>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Patient distribution retrieved successfully",

                    Data = patients
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<PatientDistribution>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve patient distribution",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<
            ApiResponse<List<AuthorizationAnalytics>>>
            GetAuthorizationAnalyticsAsync()
        {
            try
            {
                var analytics =
                    await _repository
                        .GetAuthorizationAnalyticsAsync();

                return new ApiResponse<
                    List<AuthorizationAnalytics>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Authorization analytics retrieved successfully",

                    Data = analytics
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<AuthorizationAnalytics>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve authorization analytics",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}