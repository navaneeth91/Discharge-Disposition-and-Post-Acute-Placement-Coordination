using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Insurance.Models;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Insurance.Repositories
{

    public class InsuranceRepository : IInsuranceRepository
    {
        private readonly InsuranceDbContext _context;

        public InsuranceRepository(
            InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<List<InsuranceProvider>> GetProvidersAsync()
        {
            return await _context.InsuranceProviders
                .AsNoTracking()
                .OrderBy(x => x.ProviderName)
                .ToListAsync();
        }

        public async Task<List<Plan>> GetPlansAsync(int? providerId)
        {
            IQueryable<Plan> query =
                _context.Plans
                    .AsNoTracking();

            if (providerId.HasValue)
            {
                query = query.Where(
                    x => x.InsuranceProviderId == providerId.Value);
            }

            return await query
                .OrderBy(x => x.PlanName)
                .ToListAsync();
        }
    }
}