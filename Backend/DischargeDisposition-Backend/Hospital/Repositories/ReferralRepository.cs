using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class ReferralRepository : IReferralRepository
    {
        private readonly HospitalDbContext _context;

        public ReferralRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Referral>> GetAllAsync(
                int page,
                int pageSize,
                string? search,
                string? status)
        {
            var query =
                _context.Referrals
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
            return await _context.Referrals
                .Include(r => r.patient)
                .Include(r => r.provider)
                .Include(r => r.careManager)
                .FirstOrDefaultAsync(r => r.ReferralId == id, cancellationToken);
        }

        public async Task<Referral> CreateAsync(Referral referral, CancellationToken cancellationToken = default)
        {
            _context.Referrals.Add(referral);
            await _context.SaveChangesAsync(cancellationToken);
            return referral;
        }

        public async Task UpdateAsync(Referral referral, CancellationToken cancellationToken = default)
        {
            _context.Referrals.Update(referral);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Referrals.FindAsync(new object[] { id }, cancellationToken);
            if (entity is null) return;
            _context.Referrals.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Referral>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            return await _context.Referrals
                .Where(r => r.PatientId == patientId)
                .Include(r=>r.patient)
                .Include(r => r.provider)
                .Include(r => r.careManager)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<HospitalPagedResponse<ReferralResponseDto>> GetByCareManagerIdAsync(
          int careManagerId,
          int page,
          int pageSize,
          string? search = null,
          AuthorizationStatus? status = null,
          CancellationToken cancellationToken = default)
        {
            var query = _context.Referrals
                .Include(r => r.patient)
                .Include(r => r.provider)
                .Include(r => r.careManager)
                .Where(r => r.CareManagerId == careManagerId)
                .AsNoTracking();

            // Status filter
            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(r =>
                    r.patient!.FirstName.Contains(search) ||
                    r.patient.LastName.Contains(search) ||
                    (r.patient.FirstName + " " + r.patient.LastName).Contains(search) ||
                    r.patient.Mrn.Contains(search) ||
                    r.provider!.ProviderName.Contains(search));
            }

            var totalRecords = await query.CountAsync(cancellationToken);

            var referrals = await query
                .OrderByDescending(r => r.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new ReferralResponseDto
                {
                    ReferralId = r.ReferralId,
                    PatientId = r.PatientId,
                    ProviderId = r.ProviderId,
                    CareManagerId = r.CareManagerId,
                    CreatedDate = r.CreatedDate,
                    Status = r.Status.ToString(),
                    Priority = r.Priority.ToString(),

                    PatientName = r.patient == null
                        ? null
                        : $"{r.patient.FirstName} {r.patient.LastName}",

                    ProviderName = r.provider == null
                        ? null
                        : r.provider.ProviderName,

                    CareManagerName = r.careManager == null
                        ? null
                        : $"{r.careManager.FirstName} {r.careManager.LastName}"
                })
                .ToListAsync(cancellationToken);

            return new HospitalPagedResponse<ReferralResponseDto>
            {
                Items = referrals,
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
        public async Task<IEnumerable<Referral>> GetByProviderIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            var provider =
        await _context.PostAcuteProviders
            .FirstOrDefaultAsync(
                p => p.UserId == userId);

            if (provider.ProviderId == null)
            {
                return new List<Referral>();
            }
            return await _context.Referrals
                .Where(r => r.ProviderId == provider.ProviderId)
                .Include(r => r.patient)
                .Include(r => r.careManager)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Referral>> GetPendingByProviderIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            var provider =
        await _context.PostAcuteProviders
            .FirstOrDefaultAsync(
                p => p.UserId == userId);

            if (provider.ProviderId == null)
            {
                return new List<Referral>();
            }
            return await _context.Referrals
                .Where(r => r.ProviderId == provider.ProviderId)
                .Where(r => r.Status == AuthorizationStatus.Pending)
                .Include(r => r.patient)
                .Include(r => r.careManager)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Referral>> GetPendingReferralsAsync(CancellationToken cancellationToken = default)
        {
            // Assumes AuthorizationStatus.Pending exists; adjust enum values as needed.
            return await _context.Referrals
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
            return await _context.Referrals
                .Where(r => r.Status != AuthorizationStatus.Pending)
                .Include(r => r.patient)
                .Include(r => r.provider)
                .Include(r => r.careManager)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}