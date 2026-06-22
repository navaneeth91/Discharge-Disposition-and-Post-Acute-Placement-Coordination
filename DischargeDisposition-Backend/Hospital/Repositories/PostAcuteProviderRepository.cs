using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class PostAcuteProviderRepository
        : IPostAcuteProviderRepository
    {
        private readonly HospitalDbContext _context;

        public PostAcuteProviderRepository(
            HospitalDbContext context)
        {
            _context = context;
        }
        public async Task<List<PostAcuteProvider>> GetAllAsync()
        {
            return await _context.PostAcuteProviders
                .AsNoTracking()
                .Include(p => p.dispositionType)
                .Where(p => p.IsActive)
                .OrderBy(p => p.ProviderName)
                .ToListAsync();
        }
        public async Task<PostAcuteProvider?>GetByIdAsync(int providerId)
        {
            return await _context.PostAcuteProviders
                .AsNoTracking()
                .Include(p => p.dispositionType)
                .FirstOrDefaultAsync(
                    p => p.ProviderId == providerId);
        }

        public async Task<List<PostAcuteProvider>> GetByDispositionTypeAsync(int dispositionTypeId)
        {
            return await _context.PostAcuteProviders
                .AsNoTracking()
                .Include(p => p.dispositionType)
                .Where(p =>
                    p.DispositionTypeId ==
                    dispositionTypeId &&
                    p.IsActive)
                .OrderBy(p => p.ProviderName)
                .ToListAsync();
        }
    }
}