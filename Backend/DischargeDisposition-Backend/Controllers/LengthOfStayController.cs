using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/los")]
    public class LengthOfStayController : ControllerBase
    {
        private readonly ILengthOfStayService _service;

        public LengthOfStayController(ILengthOfStayService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllLOSAsync();

            return Ok(data);
        }

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var result = await _service.GetPatientLOSAsync(patientId);

            if (result == null)
                return NotFound("No patient record found");

            return Ok(result);
        }
    }
}