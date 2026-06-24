using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [ApiController]
    [Route("api/authorizations")]
    public class AuthorizationsController
        : ControllerBase
    {
        private readonly
            IAuthorizedService _service;

        public AuthorizationsController(
            IAuthorizedService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult>
            Create(
                CreateAuthorizationRequest dto)
        {
            var response =
                await _service
                    .CreateAsync(dto);

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("{authorizationId:long}")]
        public async Task<IActionResult>
            Get(long authorizationId)
        {
            var response =
                await _service
                    .GetAsync(
                        authorizationId);

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("patient/{patientId:int}")]
        public async Task<IActionResult>
            GetPatient(int patientId)
        {
            var response =
                await _service
                    .GetPatientAuthorizationsAsync(
                        patientId);

            return this
                .ToHttpResponse(response);
        }
    }
}