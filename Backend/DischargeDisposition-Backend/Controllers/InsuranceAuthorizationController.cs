using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize(Roles = "Authorization Coordinator")]
    [ApiController]
    [Route("api/insurance-authorizations")]
    public class InsuranceAuthorizationController : ControllerBase
    {
        private readonly IInsuranceAuthorizationService _service;

        public InsuranceAuthorizationController(IInsuranceAuthorizationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthorizations(
            [FromQuery] string? search,
            [FromQuery] AuthorizationStatus? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = await _service.GetAuthorizationsAsync(search, status, page, pageSize);
            return this.ToHttpResponse(response);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentAuthorizations([FromQuery] int take = 5)
        {
            var response = await _service.GetRecentAuthorizationsAsync(take);
            return this.ToHttpResponse(response);
        }

        [HttpGet("{authorizationRequestId:int}")]
        public async Task<IActionResult> GetAuthorization(int authorizationRequestId)
        {
            var response = await _service.GetAuthorizationAsync(authorizationRequestId);
            return this.ToHttpResponse(response);
        }

        [HttpPatch("{authorizationRequestId:int}/status")]
        public async Task<IActionResult> UpdateStatus(int authorizationRequestId, UpdateAuthorizationStatus dto)
        {
            await _service.UpdateStatusAsync(authorizationRequestId, dto);
            return NoContent();
        }
    }
}