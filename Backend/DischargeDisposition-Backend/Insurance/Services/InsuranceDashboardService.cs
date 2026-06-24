using DischargeDisposition_Backend.Insurance.Repositories.Interfaces;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.Services.Interfaces;

namespace DischargeDisposition_Backend.Insurance.Services
{
    public class InsuranceDashboardService
    : IInsuranceDashboardService
    {
        private readonly IInsuranceDashboardRepository
            _repository;

        public InsuranceDashboardService(
            IInsuranceDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<InsuranceDashboard>
            GetInsuranceDashboardAsync()
        {
            return await _repository
                .GetInsuranceDashboardAsync();
        }
    }
}
