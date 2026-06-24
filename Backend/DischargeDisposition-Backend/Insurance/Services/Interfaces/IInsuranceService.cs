using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;

namespace DischargeDisposition_Backend.Insurance.Services
{
    public interface IInsuranceService
    {
        Task<ApiResponse<List<InsuranceProviderResponse>>>
            GetProvidersAsync();

        Task<ApiResponse<List<PlanResponse>>>
            GetPlansAsync(int? providerId);
    }
}