using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Hospital.Controllers
{
    [ApiController]
    [Route("api/webhooks")]
    public class WebhookController
        : ControllerBase
    {
        private readonly IAuthorizationService
            _service;

        public WebhookController(
            IAuthorizationService service)
        {
            _service = service;
        }

        [HttpPost("authorization-status")]
        public async Task<IActionResult>
            UpdateStatus(
                AuthorizationWebhook dto)
        {
            await _service
                .ProcessWebhookAsync(dto);

            return Ok();
        }
    }
}