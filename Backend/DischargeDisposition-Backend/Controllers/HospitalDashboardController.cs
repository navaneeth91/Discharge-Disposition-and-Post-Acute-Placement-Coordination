using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DischargeDisposition_Backend.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
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
            return Ok(
                await _service
                    .GetHospitalDashboardAsync());
        }
    }
}