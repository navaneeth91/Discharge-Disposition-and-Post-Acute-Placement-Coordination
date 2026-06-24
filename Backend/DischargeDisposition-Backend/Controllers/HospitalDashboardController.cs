using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DischargeDisposition_Backend.Controllers
{
    [Authorize(Roles ="Administrator,Care Manager,Physician")]
    [ApiController]
    [Route("api/dashboard")]
    
    public class DashboardController: ControllerBase
    {
        private readonly IDashboardService
            _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("hospital")]
        public async Task<IActionResult>GetHospitalDashboard()
        {
            return Ok(
                await _service
                    .GetHospitalDashboardAsync());
        }
        [HttpGet("patient-distribution")]
        public async Task<IActionResult>GetPatientDistribution()
        {
            return Ok(await _service
                    .GetPatientDistributionAsync());
        }

        [HttpGet("authorization-analytics")]
        public async Task<IActionResult>GetAuthorizationAnalytics()
        {
            return Ok(await _service
                    .GetAuthorizationAnalyticsAsync());
        }
    }
}