using DischargeDisposition_Backend.Insurance.DTOs.Responses;

namespace DischargeDisposition_Backend.Insurance.Services.Interfaces
{
    public interface IInsuranceAuthorizationService
    {
        Task UpdateStatusAsync(
            int authorizationRequestId,
            UpdateAuthorizationStatus dto);
    }
}