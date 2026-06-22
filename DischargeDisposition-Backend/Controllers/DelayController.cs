
using DischargeDisposition_Backend.DTOs.Requests;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [ApiController]
    [Route("api/patient-delays")]
    [Authorize]
    public class DelayController : ControllerBase
    {
        private readonly IPatientDelayService _patientDelayService;
        private readonly IDelayReasonCodeService _delayReasonCodeService;

        public DelayController(
            IPatientDelayService patientDelayService, IDelayReasonCodeService delayReasonCodeService)
        {
            _patientDelayService = patientDelayService;
            _delayReasonCodeService = delayReasonCodeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDelay(
            CreatePatientDelayRequest request)
        {
            var response =
                await _patientDelayService
                    .CreateAsync(request);

            return this.ToHttpResponse(response);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPatientDelays(
    int patientId)
        {
            var response =
                await _patientDelayService
                    .GetByPatientIdAsync(patientId);

            return this.ToHttpResponse(response);
        }
        [HttpGet("reason-codes")]
        public async Task<IActionResult> GetDelayReasonCodes()
        {
            var response =
                await _delayReasonCodeService.GetAllAsync();

            return this.ToHttpResponse(response);
        }
    }
}