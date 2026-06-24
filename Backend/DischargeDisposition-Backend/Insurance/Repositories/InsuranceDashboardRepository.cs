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
    }
}