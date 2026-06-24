using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

public class DashboardRepository : IDashboardRepository
{
    private readonly HospitalDbContext _context;

    public DashboardRepository(
        HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<HospitalDashboard>
        GetHospitalDashboardAsync()
    {
        return await _context.HospitalDashboard
            .AsNoTracking()
            .FirstAsync();
    }
    public async Task<List<PatientDistribution>>GetPatientDistributionAsync()
    {
        return await _context
            .PatientDistribution
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<AuthorizationAnalytics>>GetAuthorizationAnalyticsAsync()
    {
        return await _context
            .AuthorizationAnalytics
            .AsNoTracking()
            .ToListAsync();
    }
}