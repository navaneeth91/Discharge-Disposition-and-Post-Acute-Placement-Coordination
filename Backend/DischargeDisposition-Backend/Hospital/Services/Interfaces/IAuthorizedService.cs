using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IAuthorizedService
    {
        Task<ApiResponse<long>>CreateAsync(CreateAuthorizationRequest dto);

        Task<ApiResponse<AuthorizationResponse>>GetAsync(long authorizationId);

        Task<ApiResponse<List<AuthorizationResponse>>>GetPatientAuthorizationsAsync(int patientId);

        Task ProcessWebhookAsync(AuthorizationWebhook dto);
        Task<ApiResponse<HospitalPagedResponse<AuthorizationTrackingResponseDto>>> GetByCareManagerIdAsync(
        int careManagerId,
        int page,
        int pageSize,
        string? search = null,
        AuthorizationStatus? status = null,
        CancellationToken cancellationToken = default);
    }
}