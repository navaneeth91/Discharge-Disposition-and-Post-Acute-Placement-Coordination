using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class ReferralRepository : IReferralRepository
    {
        private readonly HospitalDbContext _db;

        public ReferralRepository(HospitalDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<Referral>> GetAllAsync(
                int page,
                int pageSize,
                string? search,
                string? status)
        {
            var query =
                _db.Referrals
                    .Include(x => x.patient)
                    .Include(x => x.provider)
                    .Include(x => x.careManager)
                    .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                query = query.Where(x =>

                    (x.patient.FirstName + " " +
                     x.patient.LastName)
                        .ToLower()
                        .Contains(search)

                    ||

                    x.provider.ProviderName
                        .ToLower()
                        .Contains(search)

                    ||

                    (x.careManager.FirstName + " " +
                     x.careManager.LastName)
                        .ToLower()
                        .Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x =>
                    x.Status.ToString() == status);
            }

            var totalCount =
                await query.CountAsync();

            var referrals =
                await query
                    .OrderByDescending(
                        x => x.CreatedDate)
                    .Skip(
                        (page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return new PagedResult<Referral>
            {
                Items = referrals,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages =
                    (int)Math.Ceiling(
                        totalCount /
                        (double)pageSize)
            };
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