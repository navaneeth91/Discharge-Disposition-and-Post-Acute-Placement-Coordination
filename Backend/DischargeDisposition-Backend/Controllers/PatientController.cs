using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatientService _service;

        public PatientController(ILogger<PatientController> logger, IPatientService service)
        {
            _logger = logger;
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _service.GetPatientsAsync();

            return Ok(patients);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientByIdAsync(int id)
        {
            var patient = await 
                _service.GetPatientByIdAsync(id);

            if (patient == null)
                return NotFound();

            return Ok(patient);
        }
        [HttpPatch("{id}/discharge")]
        public async Task<IActionResult> DischargePatient(int id, [FromBody] DischargePatientDto dto)
        {
            var result =
                await _service.DischargePatientAsync(id, dto.ActualDischargeDate);


            if (!result)
                return NotFound();


            return NoContent();
        }
    }
}
