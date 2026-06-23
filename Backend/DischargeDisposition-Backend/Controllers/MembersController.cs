using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize]
[ApiController]
[Route("api/members")]
public class MembersController
    : ControllerBase
{
    private readonly IMemberService _service;

    public MembersController(
        IMemberService service)
    {
        _service = service;
    }

    [HttpGet("{memberId:int}")]
    public async Task<IActionResult>
        GetMember(int memberId)
    {
        var member =
            await _service.GetMemberAsync(memberId);

        if (member == null)
            return NotFound(
                $"Member {memberId} not found.");

        return Ok(member);
    }
}