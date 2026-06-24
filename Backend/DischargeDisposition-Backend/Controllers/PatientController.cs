using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController
        : ControllerBase
    {
        private readonly ILogger<PatientController>
            _logger;

        private readonly IPatientService
            _service;

        public PatientController(
            ILogger<PatientController> logger,
            IPatientService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult>
            GetPatients()
        {
            var response =
                await _service
                    .GetPatientsAsync();

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>
            GetPatientById(int id)
        {
            var response =
                await _service
                    .GetPatientByIdAsync(id);

            return this
                .ToHttpResponse(response);
        }

        [HttpPatch("{id}/discharge")]
        public async Task<IActionResult>
            DischargePatient(
                int id,
                [FromBody]
                DischargePatientDto dto)
        {
            var response =
                await _service
                    .DischargePatientAsync(
                        id,
                        dto.ActualDischargeDate);

            return this
                .ToHttpResponse(response);
        }
    }
}