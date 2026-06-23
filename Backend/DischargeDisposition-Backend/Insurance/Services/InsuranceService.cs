using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.Repositories;

namespace DischargeDisposition_Backend.Insurance.Services
{

    public class InsuranceService : IInsuranceService
    {
        private readonly IInsuranceRepository _repository;

        public InsuranceService(
            IInsuranceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<InsuranceProviderResponse>>
            GetProvidersAsync()
        {
            var providers =
                await _repository.GetProvidersAsync();

            return providers.Select(x =>
                new InsuranceProviderResponse
                {
                    InsuranceProviderId =
                        x.InsuranceProviderId,

                    ProviderName =
                        x.ProviderName,

                    ProviderCode =
                        x.ProviderCode
                }).ToList();
        }

        public async Task<List<PlanResponse>>
            GetPlansAsync(int? providerId)
        {
            var plans =
                await _repository.GetPlansAsync(providerId);

            return plans.Select(x =>
                new PlanResponse
                {
                    PlanId = x.PlanId,
                    PlanName = x.PlanName,
                    PlanType = x.PlanType,
                    IsActive = x.IsActive
                }).ToList();
        }
    }
}