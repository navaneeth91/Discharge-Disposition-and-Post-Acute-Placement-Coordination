using Microsoft.AspNetCore.Mvc;
using DischargeDisposition_Backend.Insurance.Services;
using Microsoft.AspNetCore.Authorization;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/insurance")]
    public class InsuranceController : ControllerBase
    {
        private readonly IInsuranceService _service;

        public InsuranceController(
            IInsuranceService service)
        {
            _service = service;
        }

        [HttpGet("providers")]
        public async Task<IActionResult>
            GetProviders()
        {
            var result =
                await _service.GetProvidersAsync();

            return Ok(result);
        }

        [HttpGet("plans")]
        public async Task<IActionResult>
            GetPlans(
                [FromQuery] int? providerId)
        {
            var result =
                await _service.GetPlansAsync(providerId);

            return Ok(result);
        }
    }
}