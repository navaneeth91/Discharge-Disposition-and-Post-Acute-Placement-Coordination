using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/los")]
    public class LengthOfStayController
        : ControllerBase
    {
        private readonly
            ILengthOfStayService
            _service;

        public LengthOfStayController(
            ILengthOfStayService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAll()
        {
            var response =
                await _service
                    .GetAllLOSAsync();

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("{patientId}")]
        public async Task<IActionResult>
            GetByPatientId(
                int patientId)
        {
            var response =
                await _service
                    .GetPatientLOSAsync(
                        patientId);

            return this
                .ToHttpResponse(response);
        }
    }
}