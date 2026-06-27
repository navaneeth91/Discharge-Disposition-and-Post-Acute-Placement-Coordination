using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Insurance.Services.Interfaces
{
    public interface IInsuranceAuthorizationService
    {
        Task<ApiResponse<InsurancePagedResponse<AuthorizationRequestListItemResponse>>> GetAuthorizationsAsync(
            string? search,
            AuthorizationStatus? status,
            int page,
            int pageSize);

        Task<ApiResponse<List<AuthorizationRequestListItemResponse>>> GetRecentAuthorizationsAsync(int take);

        Task<ApiResponse<AuthorizationRequestListItemResponse>> GetAuthorizationAsync(int authorizationRequestId);

        Task UpdateStatusAsync(
            int authorizationRequestId,
            UpdateAuthorizationStatus dto);
    }
}