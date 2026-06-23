using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.Hospital.Services.Interfaces;
using System.Net.Http.Json;

namespace DischargeDisposition_Backend.Insurance.Services
{
    public class WebhookService
        : IWebhookService
    {
        private readonly HttpClient _client;

        public WebhookService(
            HttpClient client)
        {
            _client = client;
        }

        public async Task SendAuthorizationUpdateAsync(
            AuthorizationWebhook dto)
        {
            await _client.PostAsJsonAsync(
                "https://localhost:7129/api/webhooks/authorization-status",
                dto);
        }
    }
}