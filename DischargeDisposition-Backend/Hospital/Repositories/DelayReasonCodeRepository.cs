using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class DelayReasonCodeRepository : IDelayReasonCodeRepository
    {
        private readonly HospitalDbContext _context;

        public DelayReasonCodeRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DelayReasonCode>> GetAllAsync()
        {
            return await _context.DelayReasonCodes
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }
}