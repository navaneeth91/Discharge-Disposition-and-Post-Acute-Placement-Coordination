using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.Repositories.Interfaces;
using DischargeDisposition_Backend.Insurance.Services.Interfaces;

namespace DischargeDisposition_Backend.Insurance.Services
{
    public class InsuranceDashboardService
        : IInsuranceDashboardService
    {
        private readonly
            IInsuranceDashboardRepository
            _repository;

        public InsuranceDashboardService(
            IInsuranceDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<InsuranceDashboard>>
            GetInsuranceDashboardAsync()
        {
            try
            {
                var dashboard =
                    await _repository
                        .GetInsuranceDashboardAsync();

                if (dashboard == null)
                {
                    return new ApiResponse<InsuranceDashboard>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Insurance dashboard data not found"
                    };
                }

                return new ApiResponse<InsuranceDashboard>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Insurance dashboard retrieved successfully",

                    Data = dashboard
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<InsuranceDashboard>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve insurance dashboard",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<
            ApiResponse<List<InsuranceAnalytics>>>
            GetServiceAnalyticsAsync()
        {
            try
            {
                var analytics =
                    await _repository
                        .GetServiceAnalyticsAsync();

                return new ApiResponse<
                    List<InsuranceAnalytics>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Service analytics retrieved successfully",

                    Data = analytics
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<InsuranceAnalytics>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve service analytics",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<List<AuthorizationRequestListItemResponse>>> GetRecentAuthorizationRequestsAsync(int take)
        {
            try
            {
                take = take < 1 ? 5 : take;

                var requests = await _repository.GetRecentAuthorizationRequestsAsync(take);

                return new ApiResponse<List<AuthorizationRequestListItemResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Recent authorization requests retrieved successfully",
                    Data = requests
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AuthorizationRequestListItemResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve recent authorization requests",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}