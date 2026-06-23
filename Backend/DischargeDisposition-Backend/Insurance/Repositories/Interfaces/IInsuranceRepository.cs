using DischargeDisposition_Backend.Insurance.Models;

namespace DischargeDisposition_Backend.Insurance.Repositories
{

    public interface IInsuranceRepository
    {
        Task<List<InsuranceProvider>> GetProvidersAsync();

        Task<List<Plan>> GetPlansAsync(int? providerId);
    }
}