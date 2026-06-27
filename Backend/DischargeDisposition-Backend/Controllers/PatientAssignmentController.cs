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
    public class PatientAssignmentController : ControllerBase
    {
        private readonly IPatientAssignmentService _patientAssignmentService;

        public PatientAssignmentController(
            IPatientAssignmentService patientAssignmentService)
        {
            _patientAssignmentService = patientAssignmentService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignCareManager(
            [FromBody] AssignCareManagerRequest request)
        {
            var response =
                await _patientAssignmentService
                    .AssignCareManagerAsync(request);

            return this.ToHttpResponse(response);
        }

        [HttpGet("unassigned-patients")]
        public async Task<IActionResult> GetUnassignedPatients(
            int page = 1,
            int pageSize = 10,
            string? search = null)
        {
            var response =
                await _patientAssignmentService
                    .GetUnassignedPatientsAsync(
                        page,
                        pageSize,
                        search);

            return this.ToHttpResponse(response);
        }

        [HttpGet("care-managers")]
        public async Task<IActionResult> GetCareManagers()
        {
            var response =
                await _patientAssignmentService
                    .GetAllCareManagersAsync();

            return this.ToHttpResponse(response);
        }
        [Authorize(Roles ="Care Manager, Administrator")]
        [HttpGet("care-manager/{careManagerId}/patients")]
        /// <summary>
        /// Retrieves paginated patients assigned to a specific Care Manager.
        /// </summary>
    public async Task<IActionResult> GetPatientsByCareManager(
    int careManagerId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null)
        {
            var response = await _patientAssignmentService
                .GetPatientsByCareManagerAsync(
                    careManagerId,
                    page,
                    pageSize,
                    search);

            return this.ToHttpResponse(response);
        }
    }
}