using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.Repositories;

namespace DischargeDisposition_Backend.Insurance.Services
{
    public class InsuranceService
        : IInsuranceService
    {
        private readonly IInsuranceRepository
            _repository;

        public InsuranceService(
            IInsuranceRepository repository)
        {
            _repository = repository;
        }

        public async Task<
            ApiResponse<List<InsuranceProviderResponse>>>
            GetProvidersAsync()
        {
            try
            {
                var providers =
                    await _repository
                        .GetProvidersAsync();

                var result =
                    providers.Select(x =>
                        new InsuranceProviderResponse
                        {
                            InsuranceProviderId =
                                x.InsuranceProviderId,

                            ProviderName =
                                x.ProviderName,

                            ProviderCode =
                                x.ProviderCode
                        })
                    .ToList();

                if (!result.Any())
                {
                    return new ApiResponse<
                        List<InsuranceProviderResponse>>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "No insurance providers found"
                    };
                }

                return new ApiResponse<
                    List<InsuranceProviderResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Insurance providers retrieved successfully",

                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<InsuranceProviderResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve insurance providers",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<
            ApiResponse<List<PlanResponse>>>
            GetPlansAsync(int? providerId)
        {
            try
            {
                var plans =
                    await _repository
                        .GetPlansAsync(providerId);

                var result =
                    plans.Select(x =>
                        new PlanResponse
                        {
                            PlanId =
                                x.PlanId,

                            PlanName =
                                x.PlanName,

                            PlanType =
                                x.PlanType,

                            IsActive =
                                x.IsActive
                        })
                    .ToList();

                if (!result.Any())
                {
                    return new ApiResponse<
                        List<PlanResponse>>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "No plans found"
                    };
                }

                return new ApiResponse<
                    List<PlanResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Plans retrieved successfully",

                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<PlanResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve plans",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}