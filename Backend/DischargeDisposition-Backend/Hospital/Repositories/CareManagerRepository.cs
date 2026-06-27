using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class CareManagerRepository : ICareManagerRepository
    {
        private readonly HospitalDbContext _context;
        private readonly ILogger<CareManagerRepository> _logger;

        public CareManagerRepository(
            HospitalDbContext context,
            ILogger<CareManagerRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CareManagerDashboardResponse> GetDashboardAsync(int careManagerId)
        {
            try
            {
                _logger.LogInformation(
                    "Repository: Retrieving dashboard for Care Manager {CareManagerId}.",
                    careManagerId);

                // Assigned Patients
                var assignedPatients = await _context.PatientAssignments
                    .CountAsync(pa =>
                        pa.CareManagerId == careManagerId &&
                        pa.IsActive);

                // Pending Referrals
                var pendingReferrals = await _context.Referrals
                    .CountAsync(r =>
                        r.CareManagerId == careManagerId &&
                        r.Status == AuthorizationStatus.Pending);

                // Active Delays
                var activeDelays = await (
                    from pa in _context.PatientAssignments
                    join pd in _context.PatientDelays
                        on pa.PatientId equals pd.PatientId
                    where pa.CareManagerId == careManagerId
                        && pa.IsActive
                        && pd.EndDate == null
                    select pa.PatientId
                )
                .Distinct()
                .CountAsync();

                // Ready For Referral
                var readyForReferral = await (
                     from pa in _context.PatientAssignments
                     join dd in _context.DispositionDecisions
                         on pa.PatientId equals dd.PatientId
                     where pa.CareManagerId == careManagerId
                           && pa.IsActive
                           && dd.Status == AuthorizationStatus.Approved
                           && !_context.Referrals.Any(r =>
                                 r.PatientId == pa.PatientId)
                     select pa.PatientId
                 )
                 .Distinct()
                 .CountAsync();

                return new CareManagerDashboardResponse
                {
                    AssignedPatients = assignedPatients,
                    ReadyForReferral = readyForReferral,
                    PendingReferrals = pendingReferrals,
                    ActiveDelays = activeDelays
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Repository Error: Failed to retrieve Care Manager dashboard.");

                throw;
            }
        }
    }
}