using DischargeDisposition_Backend.DTOs.Requests;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.Services;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/dispositions")]
    
    public class DispositionController : ControllerBase
    {
        private readonly IDispositionTypeService _dispositionTypeService;
        private readonly IDispositionDecisionService _dispositionDecisionService;

        public DispositionController(
            IDispositionTypeService dispositionTypeService,
            IDispositionDecisionService dispositionDecisionService)
        {
            _dispositionTypeService = dispositionTypeService;
            _dispositionDecisionService = dispositionDecisionService;
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAll()
        {
            var response =
                await _dispositionTypeService.GetAllAsync();

            return this.ToHttpResponse(response);
        }

        [HttpPost("decisions")]
        public async Task<IActionResult> CreateDispositionDecision(
    CreateDispositionDecisionRequest request)
        {
            var response =
                await _dispositionDecisionService
                    .CreateAsync(request);

            return this.ToHttpResponse(response);
        }

        [HttpGet("decisionDetails/patient/{patientId}")]
        public async Task<IActionResult> GetDispositionByPatient(
    int patientId)
        {
            var response =
                await _dispositionDecisionService
                    .GetByPatientIdAsync(patientId);

            return this.ToHttpResponse(response);
        }

        [HttpPut("updateDecisions/{decisionId}")]
        public async Task<IActionResult> UpdateDispositionDecision(
    int decisionId,
    UpdateDispositionDecisionRequest request)
        {
            var response =
                await _dispositionDecisionService
                    .UpdateDecisionAsync(
                        decisionId,
                        request);

            return this.ToHttpResponse(response);
        }

        [HttpGet("assigned/patients")]
        [Authorize(Roles = "Physician")]
        public async Task<IActionResult> GetAssignedPatients()
        {
            var response =
                await _dispositionDecisionService.GetAssignedPatientsAsync();

            return StatusCode(response.StatusCode, response);
        }
    }
}