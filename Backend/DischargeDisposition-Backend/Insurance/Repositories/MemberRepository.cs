using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Insurance.DTOs.Requests;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

public class MemberRepository : IMemberRepository
{
    private readonly InsuranceDbContext _context;

    public MemberRepository(
        InsuranceDbContext context)
    {
        _context = context;
    }

    public async Task<MemberDetailsResponse?>
        GetMemberAsync(int memberId)
    {
        return await _context.Members
            .AsNoTracking()
            .Where(m => m.MemberId == memberId)
            .Select(m => new MemberDetailsResponse
            {
                MemberId = m.MemberId,
                FirstName = m.FirstName,
                LastName = m.LastName,
                PolicyNumber = m.PolicyNumber,
                Gender = m.Gender.ToString(),
                DOB = m.DOB,
                Email = m.Email,
                Phone = m.Phone,

                Coverages =
                    m.MemberCoverages
                    .Select(mc =>
                        new MemberCoverage
                        {
                            CoverageId = mc.CoverageId,
                            PlanName = mc.plan.PlanName,
                            PlanType = mc.plan.PlanType,
                            InsuranceProvider =
                                mc.plan.insuranceProvider.ProviderName
                        })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<MemberSearchResponse>> SearchMembersAsync(string query, int take)
    {
        var term = query.Trim();

        return await _context.Members
            .AsNoTracking()
            .Where(m =>
                m.FirstName.Contains(term) ||
                m.LastName.Contains(term) ||
                m.PolicyNumber.Contains(term) ||
                m.Email.Contains(term) ||
                m.Phone.Contains(term))
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .Take(take)
            .Select(m => new MemberSearchResponse
            {
                MemberId = m.MemberId,
                FirstName = m.FirstName,
                LastName = m.LastName,
                FullName = m.FirstName + " " + m.LastName,
                PolicyNumber = m.PolicyNumber,
                Gender = m.Gender,
                DOB = m.DOB,
                Email = m.Email,
                Phone = m.Phone,
                CoverageCount = m.MemberCoverages.Count,
                AuthorizationCount = m.AuthorizationRequests.Count
            })
            .ToListAsync();
    }
}