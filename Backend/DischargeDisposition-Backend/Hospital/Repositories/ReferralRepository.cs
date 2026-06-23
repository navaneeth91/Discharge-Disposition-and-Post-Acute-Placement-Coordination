using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;

using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using Microsoft.EntityFrameworkCore;
using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class ReferralRepository : IReferralRepository
    {
        private readonly HospitalDbContext _db;

        public ReferralRepository(HospitalDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Referral>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Referrals
                .Include(r => r.patient)
                .Include(r => r.provider)
                .Include(r => r.careManager)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Referral?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.Referrals
                .Include(r => r.patient)
                .Include(r => r.provider)
                .Include(r => r.careManager)
                .FirstOrDefaultAsync(r => r.ReferralId == id, cancellationToken);
        }

        public async Task<Referral> CreateAsync(Referral referral, CancellationToken cancellationToken = default)
        {
            _db.Referrals.Add(referral);
            await _db.SaveChangesAsync(cancellationToken);
            return referral;
        }

        public async Task UpdateAsync(Referral referral, CancellationToken cancellationToken = default)
        {
            _db.Referrals.Update(referral);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _db.Referrals.FindAsync(new object[] { id }, cancellationToken);
            if (entity is null) return;
            _db.Referrals.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Referral>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            return await _db.Referrals
                .Where(r => r.PatientId == patientId)
                .Include(r => r.provider)
                .Include(r => r.careManager)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Referral>> GetByProviderIdAsync(int providerId, CancellationToken cancellationToken = default)
        {
            return await _db.Referrals
                .Where(r => r.ProviderId == providerId)
                .Include(r => r.patient)
                .Include(r => r.careManager)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Referral>> GetPendingReferralsAsync(CancellationToken cancellationToken = default)
        {
            // Assumes AuthorizationStatus.Pending exists; adjust enum values as needed.
            return await _db.Referrals
                .Where(r => r.Status == AuthorizationStatus.Pending)
                .Include(r => r.patient)
                .Include(r => r.provider)
                .Include(r => r.careManager)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Referral>> GetCompletedReferralsAsync(CancellationToken cancellationToken = default)
        {
            // Treat anything not Pending as completed/closed; adjust as required by domain.
            return await _db.Referrals
                .Where(r => r.Status != AuthorizationStatus.Pending)
                .Include(r => r.patient)
                .Include(r => r.provider)
                .Include(r => r.careManager)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}