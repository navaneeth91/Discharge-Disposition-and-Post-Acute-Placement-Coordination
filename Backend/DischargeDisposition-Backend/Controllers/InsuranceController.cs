using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Insurance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/insurance")]
    public class InsuranceController
        : ControllerBase
    {
        private readonly IInsuranceService
            _service;

        public InsuranceController(
            IInsuranceService service)
        {
            _service = service;
        }

        [HttpGet("providers")]
        public async Task<IActionResult>
            GetProviders()
        {
            var response =
                await _service
                    .GetProvidersAsync();

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("plans")]
        public async Task<IActionResult>
            GetPlans(
                [FromQuery]
                int? providerId)
        {
            var response =
                await _service
                    .GetPlansAsync(providerId);

            return this
                .ToHttpResponse(response);
        }
    }
}