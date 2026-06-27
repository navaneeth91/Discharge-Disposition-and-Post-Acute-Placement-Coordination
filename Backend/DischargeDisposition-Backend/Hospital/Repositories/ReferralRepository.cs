using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
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

       public async Task<IEnumerable<Referral>> GetByProviderIdAsync(
    int userId,
    ProviderReferralQueryDto query,
    CancellationToken cancellationToken = default)
{
    var provider =
        await _db.PostAcuteProviders
            .FirstOrDefaultAsync(
                p => p.UserId == userId,
                cancellationToken);

    if (provider == null)
    {
        return new List<Referral>();
    }

    var referrals = _db.Referrals

        .Where(r =>
            r.ProviderId == provider.ProviderId)

        .Include(r => r.patient)

        .Include(r => r.careManager)

        .AsNoTracking();

    // Search

    if (!string.IsNullOrWhiteSpace(query.Search))
    {
        var search = query.Search.Trim().ToLower();

        referrals = referrals.Where(r =>

            (r.patient.FirstName + " " + r.patient.LastName)
                .ToLower()
                .Contains(search)

            ||

            r.patient.Mrn
                .ToLower()
                .Contains(search)

            ||

            r.careManager.FirstName
                .ToLower()
                .Contains(search)

            ||

            r.careManager.LastName
                .ToLower()
                .Contains(search)
        );
    }

    // Status Filter

    if (!string.IsNullOrWhiteSpace(query.Status))
    {
        referrals = referrals.Where(r =>
            r.Status.ToString() == query.Status);
    }

    // Pagination

    referrals = referrals

        .OrderByDescending(r => r.CreatedDate)

        .Skip((query.Page - 1) * query.PageSize)

        .Take(query.PageSize);

    return await referrals.ToListAsync(cancellationToken);
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

        public async Task<ReferralDetailsDto?> GetReferralDetailsAsync(int UserId,int referralId)
        {
            return await _db.Referrals

                .Where(r =>
                    r.ReferralId == referralId &&
                    r.provider.UserId == UserId)

                .Select(r => new ReferralDetailsDto
                {
                    ReferralId = r.ReferralId,
                    ReferralDate = r.CreatedDate,
                    Status = r.Status.ToString(),
                    Priority = r.Priority.ToString(),

                    PatientId = r.patient.PatientId,
                    Mrn = r.patient.Mrn,
                    PatientName = r.patient.FirstName + " " + r.patient.LastName,
                    Dob = r.patient.Dob,
                    Gender = r.patient.Gender.ToString(),
                    Email = r.patient.Email,
                    PhoneNumber = r.patient.PhoneNumber,

                    
                    ExpectedDischargeDate = r.patient.ExpectedDischargeDate,
                    

                    Department = r.patient.Department.Name,

                    ProviderName = r.provider.ProviderName,

                    CareManager =
                        r.careManager.FirstName + " " +
                        r.careManager.LastName
                })

                .FirstOrDefaultAsync();
        }

        public async Task<ProviderDashboardDto> GetDashboardSummaryAsync(int userId)
        {
            var provider = await _db.PostAcuteProviders
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (provider == null)
            {
                return new ProviderDashboardDto();
            }

            var referrals = _db.Referrals
                .Where(r => r.ProviderId == provider.ProviderId);

            return new ProviderDashboardDto
            {
                TotalReferrals = await referrals.CountAsync(),

                PendingReferrals = await referrals.CountAsync(r =>
                    r.Status == AuthorizationStatus.Pending),

                ApprovedReferrals = await referrals.CountAsync(r =>
                    r.Status == AuthorizationStatus.Approved),

                RejectedReferrals = await referrals.CountAsync(r =>
                    r.Status == AuthorizationStatus.Denied)
            };
        }
    }
}