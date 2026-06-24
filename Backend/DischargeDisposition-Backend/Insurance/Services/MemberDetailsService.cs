using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;

public class MemberService
    : IMemberService
{
    private readonly IMemberRepository
        _repository;

    public MemberService(
        IMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<
        ApiResponse<MemberDetailsResponse>>
        GetMemberAsync(int memberId)
    {
        try
        {
            var member =
                await _repository
                    .GetMemberAsync(memberId);

            if (member == null)
            {
                return new ApiResponse<
                    MemberDetailsResponse>
                {
                    Success = false,
                    StatusCode = 404,
                    Message =
                        $"Member {memberId} not found"
                };
            }

            return new ApiResponse<
                MemberDetailsResponse>
            {
                Success = true,
                StatusCode = 200,
                Message =
                    "Member retrieved successfully",

                Data = member
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<
                MemberDetailsResponse>
            {
                Success = false,
                StatusCode = 500,
                Message =
                    "Failed to retrieve member",

                Errors = new()
                {
                    ex.Message
                }
            };
        }
    }
}