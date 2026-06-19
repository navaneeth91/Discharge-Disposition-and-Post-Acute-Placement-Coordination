using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{

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
    }
}
