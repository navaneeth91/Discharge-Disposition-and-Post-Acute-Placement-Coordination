using DischargeDisposition_Backend.Insurance.DTOs.Responses;

namespace DischargeDisposition_Backend.Insurance.Services
{

    public interface IInsuranceService
    {
        Task<List<InsuranceProviderResponse>>
            GetProvidersAsync();

        Task<List<PlanResponse>>
            GetPlansAsync(int? providerId);
    }
}