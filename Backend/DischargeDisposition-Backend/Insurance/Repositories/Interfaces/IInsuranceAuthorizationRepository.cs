using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Insurance.Models;

namespace DischargeDisposition_Backend.Insurance.Repositories.Interfaces
{
    public interface IInsuranceAuthorizationRepository
    {
        Task<(List<AuthorizationRequest> Items, int TotalCount)> GetAuthorizationsAsync(
            string? search,
            AuthorizationStatus? status,
            int page,
            int pageSize);

        Task<AuthorizationRequest?> GetByIdAsync(int authorizationRequestId);

        Task<AuthorizationRequest?> GetByIdWithTrackingAsync(int authorizationRequestId);
        
    }
}