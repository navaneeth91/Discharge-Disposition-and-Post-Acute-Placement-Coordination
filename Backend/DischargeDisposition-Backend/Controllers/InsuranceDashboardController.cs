
using DischargeDisposition_Backend.Insurance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DischargeDisposition_Backend.Controllers
{
    [Authorize(Roles ="Authorization Coordinator")]
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
        public async Task<IActionResult> GetInsuranceDashboard()
        {
            return Ok(
                await _service
                    .GetInsuranceDashboardAsync());
        }
        [HttpGet("service-analytics")]
        public async Task<IActionResult>GetServiceAnalytics()
        {
            return Ok(await _service
                    .GetServiceAnalyticsAsync());
        }
    }
}