using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class DispositionsTypeRepository : IDispositionTypeRepository
    {
        private readonly HospitalDbContext _context;

        public DispositionsTypeRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<List<DispositionType>> GetAllAsync()
        {
            return await _context.DispositionTypes
                .AsNoTracking()
                .OrderBy(x => x.DispositionName)
                .ToListAsync();
        }
    }
}