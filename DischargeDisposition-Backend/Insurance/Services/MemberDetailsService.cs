using DischargeDisposition_Backend.Insurance.DTOs.Responses;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _repository;

    public MemberService(
        IMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<MemberDetailsResponse?>
        GetMemberAsync(int memberId)
    {
        return await _repository.GetMemberAsync(memberId);
    }
}