using DischargeDisposition_Backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/members")]
    public class MembersController
        : ControllerBase
    {
        private readonly IMemberService
            _service;

        public MembersController(
            IMemberService service)
        {
            _service = service;
        }

        [HttpGet("{memberId:int}")]
        public async Task<IActionResult>
            GetMember(int memberId)
        {
            var response =
                await _service
                    .GetMemberAsync(memberId);

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMembers([FromQuery] string query, [FromQuery] int take = 20)
        {
            var response = await _service.SearchMembersAsync(query, take);

            return this.ToHttpResponse(response);
        }
    }
}