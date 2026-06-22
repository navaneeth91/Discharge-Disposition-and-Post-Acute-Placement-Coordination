using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IAuthorizationService
    {
        Task<long> CreateAsync(CreateAuthorizationRequest dto);

        Task<AuthorizationResponse?>GetAsync(long authorizationId);

        Task<List<AuthorizationResponse>>GetPatientAuthorizationsAsync(int patientId);

        Task ProcessWebhookAsync(AuthorizationWebhook dto);
    }
}