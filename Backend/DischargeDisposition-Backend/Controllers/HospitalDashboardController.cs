using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize(
        Roles =
        "Administrator,Care Manager,Physician")]
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController
        : ControllerBase
    {
        private readonly IDashboardService
            _service;

        public DashboardController(
            IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("hospital")]
        public async Task<IActionResult>
            GetHospitalDashboard()
        {
            var response =
                await _service
                    .GetHospitalDashboardAsync();

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("patient-distribution")]
        public async Task<IActionResult>
            GetPatientDistribution()
        {
            var response =
                await _service
                    .GetPatientDistributionAsync();

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("authorization-analytics")]
        public async Task<IActionResult>
            GetAuthorizationAnalytics()
        {
            var response =
                await _service
                    .GetAuthorizationAnalyticsAsync();

            return this
                .ToHttpResponse(response);
        }
    }
}