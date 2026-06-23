using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Insurance.Hospital.Services.Interfaces
{
    public interface IWebhookService
    {
        Task SendAuthorizationUpdateAsync(
            AuthorizationWebhook dto);
    }
}