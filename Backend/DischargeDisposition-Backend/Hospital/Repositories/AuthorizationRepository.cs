using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
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
                .Include(x => x.referral)
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
        public async Task<HospitalPagedResponse<AuthorizationTrackingResponseDto>>
    GetByCareManagerIdAsync(
    int careManagerId,
    int page,
    int pageSize,
    string? search = null,
    AuthorizationStatus? status = null,
    CancellationToken cancellationToken = default)
        {
            var query = _context.AuthorizationTrackings
                .AsNoTracking()
                .Include(a => a.patient)
                .Include(a => a.payer)
                .Include(a => a.referral)
                .Where(a => a.referral.CareManagerId == careManagerId);

            // Status Filter
            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(a =>
                    a.patient.FirstName.Contains(search) ||
                    a.patient.LastName.Contains(search) ||
                    (a.patient.FirstName + " " + a.patient.LastName)
                        .Contains(search) ||
                    a.patient.Mrn.Contains(search) ||
                    a.payer.PayerName.Contains(search) ||
                    a.ExternalAuthorizationId.Contains(search));
            }

            var totalRecords = await query.CountAsync(cancellationToken);

            var authorizations = await query
                .OrderByDescending(a => a.RequestedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AuthorizationTrackingResponseDto
                {
                    AuthorizationId = a.AuthorizationId,

                    ReferralId = a.ReferralId,

                    PatientId = a.PatientId,

                    PatientName = a.patient.FirstName + " " + a.patient.LastName,

                    MRN = a.patient.Mrn,

                    PayerName = a.payer.PayerName,

                    ExternalAuthorizationId = a.ExternalAuthorizationId,

                    Status = a.Status,

                    RequestedDate = a.RequestedDate,

                    ResponseDate = a.ResponseDate,

                    DenialReason = a.DenialReason,

                    LastUpdated = a.LastUpdated
                })
                .ToListAsync(cancellationToken);

            return new HospitalPagedResponse<AuthorizationTrackingResponseDto>
            {
                Items = authorizations,
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
