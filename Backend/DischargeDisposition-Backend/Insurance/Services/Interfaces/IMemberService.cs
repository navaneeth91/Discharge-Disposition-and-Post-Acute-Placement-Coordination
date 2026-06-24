using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;

public interface IMemberService
{
    Task<ApiResponse<MemberDetailsResponse>>
        GetMemberAsync(int memberId);
}