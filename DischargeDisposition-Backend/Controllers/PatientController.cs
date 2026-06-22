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
            var patients = _service.GetPatients();

            return Ok(patients);
        }
        [HttpGet("{id}")]
        public IActionResult GetPatientById(int id)
        {
            var patient =
                _service.GetPatientById(id);

            if (patient == null)
                return NotFound();

            return Ok(patient);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(
    int id,
    UpdateUserDto dto)
        {
            var result =
                await _service.UpdatePatientAsync(
                    id,
                    dto);

            if (!result)
                return NotFound();

            return Ok();
        }
    }
}
