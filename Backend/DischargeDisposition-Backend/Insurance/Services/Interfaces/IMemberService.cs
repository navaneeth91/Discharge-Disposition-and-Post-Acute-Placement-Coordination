using DischargeDisposition_Backend.Insurance.DTOs.Responses;

public interface IMemberService
{
    Task<MemberDetailsResponse?>
        GetMemberAsync(int memberId);
}