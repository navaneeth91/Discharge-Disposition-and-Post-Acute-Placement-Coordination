using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using Microsoft.EntityFrameworkCore;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;

public class LengthOfStayRepository : ILengthOfStayRepository
{
    private readonly HospitalDbContext _context;

    public LengthOfStayRepository(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<LengthOfStayTracking?> GetLosByPatientIdAsync(int patientId)
    {
        return await _context.LengthOfStayTrackings
            .Include(x => x.patient)
            .FirstOrDefaultAsync(x => x.PatientId == patientId);
    }

    public async Task<List<LengthOfStayTracking>> GetAllLosAsync()
    {
        return await _context.LengthOfStayTrackings
            .Include(x => x.patient)
            .ToListAsync();
    }
}