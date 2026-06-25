using DischargeDisposition_Backend.Insurance.DTOs.Responses;

public interface IMemberRepository
{
    Task<MemberDetailsResponse?>
        GetMemberAsync(int memberId);

    Task<List<MemberSearchResponse>> SearchMembersAsync(string query, int take);
}