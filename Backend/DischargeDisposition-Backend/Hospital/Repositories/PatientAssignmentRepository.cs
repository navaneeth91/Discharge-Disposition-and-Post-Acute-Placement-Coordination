using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class PatientAssignmentRepository : IPatientAssignmentRepository
    {
        private readonly HospitalDbContext _context;
        private readonly ILogger<PatientAssignmentRepository> _logger;

        public PatientAssignmentRepository(
            HospitalDbContext context,
            ILogger<PatientAssignmentRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PatientAssignment> AssignCareManagerAsync(PatientAssignment assignment)
        {
            try
            {
                _logger.LogInformation(
                    "Repository: Assigning Care Manager {CareManagerId} to Patient {PatientId}.",
                    assignment.CareManagerId,
                    assignment.PatientId);

                _context.PatientAssignments.Add(assignment);

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Repository: Patient assignment created successfully.");

                return assignment;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Repository Error: Failed to assign Care Manager.");

                throw;
            }
        }

        public async Task<PagedResult<Patient>> GetUnassignedPatientsAsync(
            int page,
            int pageSize,
            string? search)
        {
            try
            {
                _logger.LogInformation(
                    "Repository: Retrieving unassigned patients. Page: {Page}, PageSize: {PageSize}, Search: {Search}",
                    page,
                    pageSize,
                    search);

                var query =
                    _context.Patients
                        .Include(p => p.Department)
                        .Where(p => p.IsActive == 1)
                        .Where(p =>
                            !_context.PatientAssignments.Any(pa =>
                                pa.PatientId == p.PatientId &&
                                pa.IsActive))
                        .AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();

                    query = query.Where(p =>

                        p.FirstName.Contains(search) ||

                        p.LastName.Contains(search) ||

                        p.Mrn.Contains(search) ||

                        p.Department.Name.Contains(search)

                    );
                }

                var totalCount =
                    await query.CountAsync();

                var patients =
                    await query
                        .OrderBy(p => p.FirstName)
                        .ThenBy(p => p.LastName)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                _logger.LogInformation(
                    "Repository: Retrieved {Count} patients out of {TotalCount}.",
                    patients.Count,
                    totalCount);

                return new PagedResult<Patient>
                {
                    Items = patients,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages =
                        (int)Math.Ceiling(
                            totalCount /
                            (double)pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Repository Error: Failed to retrieve unassigned patients.");

                throw;
            }
        }

        public async Task<PagedResponse<AssignedPatientDto>> GetPatientsByCareManagerAsync(
          int careManagerId,
          int page,
          int pageSize,
          string? search = null)
        {
            try
            {
                _logger.LogInformation(
                    "Repository: Retrieving patients for Care Manager {CareManagerId}. Page: {Page}, PageSize: {PageSize}, Search: {Search}",
                    careManagerId,
                    page,
                    pageSize,
                    search);

                var query = _context.PatientAssignments
                    .Where(pa =>
                        pa.CareManagerId == careManagerId &&
                        pa.IsActive);

                // Search by Patient Name or MRN
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();

                    query = query.Where(pa =>
                        pa.Patient.FirstName.Contains(search) ||
                        pa.Patient.LastName.Contains(search) ||
                        (pa.Patient.FirstName + " " + pa.Patient.LastName)
                            .Contains(search) ||
                        pa.Patient.Mrn.Contains(search));
                }

                var totalRecords = await query.CountAsync();

                var patients = await query
                    .OrderBy(pa => pa.Patient.PatientId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(pa => new AssignedPatientDto
                    {
                        PatientId = pa.Patient.PatientId,

                        Mrn = pa.Patient.Mrn,

                        PatientName = pa.Patient.FirstName + " " + pa.Patient.LastName,

                        Gender = pa.Patient.Gender.ToString(),

                        DepartmentName = pa.Patient.Department.Name,

                        ExpectedDischargeDate = pa.Patient.ExpectedDischargeDate,

                        DispositionTypeId = _context.DispositionDecisions
                            .Where(dd => dd.PatientId == pa.PatientId)
                            .Select(dd => (int?)dd.DispositionTypeId)
                            .FirstOrDefault(),

                        DispositionType = _context.DispositionDecisions
                            .Where(dd => dd.PatientId == pa.PatientId)
                            .Select(dd => dd.dispositionType.DispositionName)
                            .FirstOrDefault(),

                        HasReferral = _context.Referrals
                            .Any(r => r.PatientId == pa.PatientId),

                        ReferralStatus = _context.Referrals
                            .Where(r => r.PatientId == pa.PatientId)
                            .Select(r => r.Status.ToString())
                            .FirstOrDefault()
                    })
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation(
                    "Repository: Retrieved {Count} patients out of {Total}.",
                    patients.Count,
                    totalRecords);

                return new PagedResponse<AssignedPatientDto>
                {
                    Items = patients,
                    Page = page,
                    PageSize = pageSize,
                    TotalRecords = totalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Repository Error: Failed to retrieve assigned patients.");

                throw;
            }
        }

        public async Task<bool> IsPatientAssignedAsync(int patientId)
        {
            try
            {
                return await _context.PatientAssignments
                    .AsNoTracking()
                    .AnyAsync(pa =>
                        pa.PatientId == patientId &&
                        pa.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Repository Error: Failed to check patient assignment.");

                throw;
            }
        }

        public async Task<PatientAssignment?> GetAssignmentByPatientIdAsync(int patientId)
        {
            try
            {
                return await _context.PatientAssignments
                    .Include(pa => pa.Patient)
                    .Include(pa => pa.CareManager)
                    .FirstOrDefaultAsync(pa =>
                        pa.PatientId == patientId &&
                        pa.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Repository Error: Failed to retrieve patient assignment.");

                throw;
            }
        }

        public async Task<PatientAssignment> UpdateAssignmentAsync(PatientAssignment assignment)
        {
            try
            {
                _context.PatientAssignments.Update(assignment);

                await _context.SaveChangesAsync();

                return assignment;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Repository Error: Failed to update patient assignment.");

                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllCareManagersAsync()
        {
            try
            {
                _logger.LogInformation(
                    "Repository: Retrieving all Care Managers.");

                var careManagers = await _context.Users
                    .Include(u => u.department)
                    .Include(u => u.role)
                    .Where(u => u.role.Name == "Care Manager")
                    .AsNoTracking()
                    .ToListAsync();

                return careManagers;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Repository Error: Failed to retrieve Care Managers.");

                throw;
            }
        }

        public async Task<bool> CareManagerExistsAsync(int careManagerId)
        {
            try
            {
                return await _context.Users
                    .AsNoTracking()
                    .AnyAsync(u =>
                        u.UserId == careManagerId &&
                        u.role.Name == "Care Manager");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Repository Error: Failed to verify Care Manager.");

                throw;
            }
        }
    }
}