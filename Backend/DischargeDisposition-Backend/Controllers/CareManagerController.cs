using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CareManagerController : ControllerBase
    {
        private readonly ICareManagerService _careManagerService;

        public CareManagerController(
            ICareManagerService careManagerService)
        {
            _careManagerService = careManagerService
                ?? throw new ArgumentNullException(nameof(careManagerService));
        }

        [HttpGet("dashboard/{careManagerId}")]
        public async Task<IActionResult> GetDashboard(int careManagerId)
        {
            var response =
                await _careManagerService.GetDashboardAsync(careManagerId);

            return this.ToHttpResponse(response);
        }
    }
}