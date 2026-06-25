using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Insurance.Models;
using DischargeDisposition_Backend.Insurance.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Insurance.Repositories
{
    public class InsuranceAuthorizationRepository : IInsuranceAuthorizationRepository
    {
        private readonly InsuranceDbContext _context;

        public InsuranceAuthorizationRepository(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<(List<AuthorizationRequest> Items, int TotalCount)> GetAuthorizationsAsync(
            string? search,
            AuthorizationStatus? status,
            int page,
            int pageSize)
        {
            IQueryable<AuthorizationRequest> query = _context.AuthorizationRequests
                .AsNoTracking()
                .Include(x => x.member)
                .Include(x => x.AuthorizationDecisions);

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(x =>
                    x.member.FirstName.Contains(term) ||
                    x.member.LastName.Contains(term) ||
                    x.member.PolicyNumber.Contains(term) ||
                    x.RequestingOrganization.Contains(term) ||
                    x.ServiceType.Contains(term));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.RequestedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<AuthorizationRequest?> GetByIdAsync(int authorizationRequestId)
        {
            return await _context.AuthorizationRequests
                .AsNoTracking()
                .Include(x => x.member)
                .Include(x => x.AuthorizationDecisions)
                .FirstOrDefaultAsync(x => x.AuthorizationRequestId == authorizationRequestId);
        }

        public async Task<AuthorizationRequest?> GetByIdWithTrackingAsync(int authorizationRequestId)
        {
            return await _context.AuthorizationRequests
                .Include(x => x.member)
                .Include(x => x.AuthorizationDecisions)
                .FirstOrDefaultAsync(x => x.AuthorizationRequestId == authorizationRequestId);
        }
    }
}