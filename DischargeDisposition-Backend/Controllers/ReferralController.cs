using Microsoft.AspNetCore.Mvc;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;


namespace DischargeDisposition_Backend.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/referrals")]
    public class ReferralController : ControllerBase
    {
        private readonly IReferralService _service;
        private readonly ILogger<ReferralController> _logger;

        public ReferralController(IReferralService service, ILogger<ReferralController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var items = await _service.GetAllAsync(cancellationToken);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAll");
                return StatusCode(500, "An error occurred while fetching referrals.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _service.GetByIdAsync(id, cancellationToken);
                if (item is null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetById");
                return StatusCode(500, "An error occurred while fetching the referral.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReferralDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.ReferralId }, created);
            }
            catch (KeyNotFoundException k)
            {
                return BadRequest(new { Message = k.Message });
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { Message = aex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Create");
                return StatusCode(500, "An error occurred while creating the referral.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReferralDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var ok = await _service.UpdateAsync(id, dto, cancellationToken);
                if (!ok) return NotFound();
                return Ok();
            }
            catch (KeyNotFoundException k)
            {
                return BadRequest(new { Message = k.Message });
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { Message = aex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Update");
                return StatusCode(500, "An error occurred while updating the referral.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                var ok = await _service.DeleteAsync(id, cancellationToken);
                if (!ok) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete");
                return StatusCode(500, "An error occurred while deleting the referral.");
            }
        }

        [HttpGet("patient/{patientId:int}")]
        public async Task<IActionResult> GetByPatientId(int patientId, CancellationToken cancellationToken)
        {
            try
            {
                var items = await _service.GetByPatientIdAsync(patientId, cancellationToken);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByPatientId");
                return StatusCode(500, "An error occurred.");
            }
        }

        [HttpGet("provider/{providerId:int}")]
        public async Task<IActionResult> GetByProviderId(int providerId, CancellationToken cancellationToken)
        {
            try
            {
                var items = await _service.GetByProviderIdAsync(providerId, cancellationToken);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByProviderId");
                return StatusCode(500, "An error occurred.");
            }
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPending(CancellationToken cancellationToken)
        {
            try
            {
                var items = await _service.GetPendingReferralsAsync(cancellationToken);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPending");
                return StatusCode(500, "An error occurred.");
            }
        }

        [HttpGet("completed")]
        public async Task<IActionResult> GetCompleted(CancellationToken cancellationToken)
        {
            try
            {
                var items = await _service.GetCompletedReferralsAsync(cancellationToken);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCompleted");
                return StatusCode(500, "An error occurred.");
            }
        }
    }
}