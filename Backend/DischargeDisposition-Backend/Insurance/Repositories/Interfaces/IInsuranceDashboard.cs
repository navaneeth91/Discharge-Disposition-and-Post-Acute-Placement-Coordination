using DischargeDisposition_Backend.Insurance.DTOs.Responses;
namespace DischargeDisposition_Backend.Insurance.Repositories.Interfaces
{
    public interface IInsuranceDashboardRepository
    {
        Task<InsuranceDashboard>
            GetInsuranceDashboardAsync();
    }
}
