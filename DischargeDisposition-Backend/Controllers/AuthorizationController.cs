using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Hospital.Controllers
{
    [ApiController]
    [Route("api/authorizations")]
    public class AuthorizationsController
        : ControllerBase
    {
        private readonly IAuthorizationService
            _service;

        public AuthorizationsController(
            IAuthorizationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult>
            Create(
                CreateAuthorizationRequest dto)
        {
            var id =
                await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(Get),
                new { authorizationId = id },
                null);
        }

        [HttpGet("{authorizationId:long}")]
        public async Task<IActionResult>
            Get(int authorizationId)
        {
            var result =
                await _service.GetAsync(
                    authorizationId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("patient/{patientId:int}")]
        public async Task<IActionResult>
            GetPatient(int patientId)
        {
            var result =
                await _service
                    .GetPatientAuthorizationsAsync(
                        patientId);

            return Ok(result);
        }
    }
}