using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Insurance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize(
        Roles = "Authorization Coordinator")]
    [ApiController]
    [Route("api/insurance-dashboard")]
    public class InsuranceDashboardController
        : ControllerBase
    {
        private readonly
            IInsuranceDashboardService
            _service;

        public InsuranceDashboardController(
            IInsuranceDashboardService service)
        {
            _service = service;
        }

        [HttpGet("insurance")]
        public async Task<IActionResult>
            GetInsuranceDashboard()
        {
            var response =
                await _service
                    .GetInsuranceDashboardAsync();

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("service-analytics")]
        public async Task<IActionResult>
            GetServiceAnalytics()
        {
            var response =
                await _service
                    .GetServiceAnalyticsAsync();

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("recent-authorizations")]
        public async Task<IActionResult> GetRecentAuthorizations([FromQuery] int take = 5)
        {
            var response = await _service.GetRecentAuthorizationRequestsAsync(take);

            return this.ToHttpResponse(response);
        }
    }
}