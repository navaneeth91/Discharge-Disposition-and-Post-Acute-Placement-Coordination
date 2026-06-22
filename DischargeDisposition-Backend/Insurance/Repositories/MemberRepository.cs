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
}