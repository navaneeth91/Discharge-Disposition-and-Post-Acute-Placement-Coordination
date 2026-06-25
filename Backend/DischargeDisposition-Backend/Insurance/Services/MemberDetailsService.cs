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

    public async Task<ApiResponse<List<MemberSearchResponse>>> SearchMembersAsync(string query, int take)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new ApiResponse<List<MemberSearchResponse>>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Search query is required"
                };
            }

            take = take < 1 ? 20 : take;

            var members = await _repository.SearchMembersAsync(query, take);

            return new ApiResponse<List<MemberSearchResponse>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Members retrieved successfully",
                Data = members
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<MemberSearchResponse>>
            {
                Success = false,
                StatusCode = 500,
                Message = "Failed to search members",
                Errors = new()
                {
                    ex.Message
                }
            };
        }
    }
    }
