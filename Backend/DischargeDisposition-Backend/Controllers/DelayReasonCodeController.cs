using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/delay-reason-codes")]
    public class DelayReasonCodeController : ControllerBase
    {
        private readonly IDelayReasonCodeService _service;

        public DelayReasonCodeController(
            IDelayReasonCodeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }
    }
}