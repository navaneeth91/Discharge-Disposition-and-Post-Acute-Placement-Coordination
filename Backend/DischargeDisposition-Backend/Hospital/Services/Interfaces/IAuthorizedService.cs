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
    }
}