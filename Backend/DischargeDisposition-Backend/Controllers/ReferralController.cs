using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAll(
            int page = 1,
            int pageSize = 10,
            string? search = null,
            string? status = null,
            CancellationToken cancellationToken = default)
        {
            var response =
                await _service.GetAllAsync(
                    page,
                    pageSize,
                    search,
                    status,
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
                CancellationToken cancellationToken)
        {
            var response =
                await _service.GetByProviderIdAsync(
                    userId,
                    cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("provider/pending/{userId:int}")]
        public async Task<IActionResult>
            GetPendingByProviderId(
                int userId,
                CancellationToken cancellationToken)
        {
            var response =
                await _service.GetPendingByProviderIdAsync(
                    userId,
                    cancellationToken);

            return this
                .ToHttpResponse(response);
        }

        [HttpPatch("/provider/{id}/accept")]
        public async Task<IActionResult> AcceptReferral(int id)
        {
            var response = await _service.AcceptReferralAsync(id);

            return this.ToHttpResponse(response);
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


        
    }
}