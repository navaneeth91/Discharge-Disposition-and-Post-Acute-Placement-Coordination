using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class AuthorizationRepository
    : IAuthorizationRepository
    {
        private readonly HospitalDbContext _context;

        public AuthorizationRepository(
            HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<AuthorizationTracking?>
            GetAsync(long id)
        {
            return await _context.AuthorizationTrackings
                .AsNoTracking()
                .Include(a => a.patient)
                .Include(a => a.payer)
                .FirstOrDefaultAsync(
                    a => a.AuthorizationId == id);
        }

        public async Task<List<AuthorizationTracking>>
        GetByPatientAsync(int patientId)
        {
            return await _context.AuthorizationTrackings
                .AsNoTracking()
                .Where(x => x.PatientId == patientId)
                .Include(x => x.payer)
                .Include(x => x.patient)
                .ToListAsync();
        }

        public async Task<AuthorizationTracking?>
            GetByExternalIdAsync(string externalId)
        {
            return await _context.AuthorizationTrackings
                .FirstOrDefaultAsync(
                    x => x.ExternalAuthorizationId == externalId);
        }
        public async Task<AuthorizationTracking?>GetByInsuranceRequestIdAsync(int authorizationRequestId)
        {
            return await _context.AuthorizationTrackings
                .FirstOrDefaultAsync(x =>
                    x.InsuranceAuthorizationRequestId ==
                    authorizationRequestId);
        }
        public async Task AddAsync(
            AuthorizationTracking authorization)
        {
            await _context.AuthorizationTrackings
                .AddAsync(authorization);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
