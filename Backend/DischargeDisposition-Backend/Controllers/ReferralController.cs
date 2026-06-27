using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Services;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;

namespace DischargeDisposition_Backend.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/referrals")]
    public class ReferralController
        : ControllerBase
    {
        private readonly
            IReferralService _service;

        public ReferralController(
            IReferralService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAll(
                CancellationToken cancellationToken)
        {
            var response =
                await _service.GetAllAsync(
                    cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult>
            GetById(
                int id,
                CancellationToken cancellationToken)
        {
            var response =
                await _service.GetByIdAsync(
                    id,
                    cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult>
            Create(
                CreateReferralDto dto,
                CancellationToken cancellationToken)
        {
            var response =
                await _service.CreateAsync(
                    dto,
                    cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult>
            Update(
                int id,
                UpdateReferralDto dto,
                CancellationToken cancellationToken)
        {
            var response =
                await _service.UpdateAsync(
                    id,
                    dto,
                    cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult>
            UpdateStatus(
                int id,
                AuthorizationStatus status,
                CancellationToken cancellationToken)
        {
            var response =
                await _service.UpdateStatusAsync(
                    id,
                    status,
                    cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult>
            Delete(
                int id,
                CancellationToken cancellationToken)
        {
            var response =
                await _service.DeleteAsync(
                    id,
                    cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("patient/{patientId:int}")]
        public async Task<IActionResult>
            GetByPatientId(
                int patientId,
                CancellationToken cancellationToken)
        {
            var response =
                await _service.GetByPatientIdAsync(
                    patientId,
                    cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("provider/{userId:int}")]
        public async Task<IActionResult>
            GetByProviderId(
                int userId,
                [FromQuery] ProviderReferralQueryDto query,CancellationToken cancellationToken)
        {
            var response =
                await _service.GetByProviderIdAsync(
                    userId,
                    query,cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        


        [HttpGet("pending")]
        public async Task<IActionResult>
            GetPending(
                CancellationToken cancellationToken)
        {
            var response =
                await _service.GetPendingReferralsAsync(
                    cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("completed")]
        public async Task<IActionResult>
            GetCompleted(
                CancellationToken cancellationToken)
        {
            var response =
                await _service.GetCompletedReferralsAsync(
                    cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("details/{referralId}")]
        public async Task<IActionResult> GetReferralDetails(int referralId)
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int UserId = int.Parse(userIdClaim);

            var response =
                await _service.GetReferralDetailsAsync(
                    UserId,
                    referralId);

            return StatusCode(
                response.StatusCode,
                response);
        }


        [HttpGet("provider/dashboard")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId =
                int.Parse(userIdClaim);

            var response =
                await _service.GetDashboardSummaryAsync(userId);

            return StatusCode(
                response.StatusCode,
                response);
        }

    }
}