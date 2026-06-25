using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Insurance.Repositories.Interfaces;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Insurance.Repositories
{
    public class InsuranceDashboardRepository
        : IInsuranceDashboardRepository
    {
        private readonly InsuranceDbContext _context;

        public InsuranceDashboardRepository(
            InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<InsuranceDashboard> GetInsuranceDashboardAsync()
        {
            return await _context.InsuranceDashboard
                .AsNoTracking()
                .FirstAsync();
        }

        public async Task<List<InsuranceAnalytics>>GetServiceAnalyticsAsync()
        {
            return await _context.InsuranceAnalytics
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<AuthorizationRequestListItemResponse>> GetRecentAuthorizationRequestsAsync(int take)
        {
            return await _context.AuthorizationRequests
                .AsNoTracking()
                .Include(x => x.member)
                .Include(x => x.AuthorizationDecisions)
                .OrderByDescending(x => x.RequestedDate)
                .Take(take)
                .Select(x => new AuthorizationRequestListItemResponse
                {
                    AuthorizationRequestId = x.AuthorizationRequestId,
                    MemberId = x.MemberId,
                    MemberName = x.member.FirstName + " " + x.member.LastName,
                    PolicyNumber = x.member.PolicyNumber,
                    RequestingOrganization = x.RequestingOrganization,
                    ServiceType = x.ServiceType,
                    RequestedDate = x.RequestedDate,
                    Status = x.Status,
                    LatestDecisionDate = x.AuthorizationDecisions
                        .OrderByDescending(d => d.DecisionDate)
                        .Select(d => (DateTime?)d.DecisionDate)
                        .FirstOrDefault(),
                    ReasonCode = x.AuthorizationDecisions
                        .OrderByDescending(d => d.DecisionDate)
                        .Select(d => d.ReasonCode)
                        .FirstOrDefault(),
                    Notes = x.AuthorizationDecisions
                        .OrderByDescending(d => d.DecisionDate)
                        .Select(d => d.Notes)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }
    }
}