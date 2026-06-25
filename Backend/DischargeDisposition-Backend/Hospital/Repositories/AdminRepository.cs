using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly HospitalDbContext _context;
        private readonly ILogger<AdminRepository> _logger;
        public AdminRepository(HospitalDbContext context, ILogger<AdminRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<PagedResult<User>>GetAllUsersAsync(
        int page,
        int pageSize,
        string? search)
        {
            var query =
                _context.Users
                    .Include(x => x.department)
                    .Include(x => x.role)
                    .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.FirstName.Contains(search)
                    ||
                    x.LastName.Contains(search)
                    ||
                    x.Email.Contains(search));
            }

            var totalCount =
                await query.CountAsync();

            var users =
                await query
                    .OrderBy(x => x.FirstName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return new PagedResult<User>
            {
                Items = users,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages =
                    (int)Math.Ceiling(
                        totalCount /
                        (double)pageSize)
            };
        }
        /// <summary>
        /// Retrieves a specific user by ID with related information.
        /// </summary>
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            try
            {
                _logger.LogInformation($"Repository: Retrieving user with ID {userId} from database.");

                if (userId <= 0)
                {
                    _logger.LogWarning($"Repository: Invalid user ID provided: {userId}");
                    return null;
                }

                var user = await _context.Users
                    .Include(u => u.department)
                    .Include(u => u.role)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    _logger.LogWarning($"Repository: User with ID {userId} not found in database.");
                }
                else
                {
                    _logger.LogInformation($"Repository: Successfully retrieved user with ID {userId}.");
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository Error: Failed to retrieve user with ID {userId}. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<User>UpdateUserAsync(User user)
        {
            try
            {
                await _context
                    .SaveChangesAsync();

                await _context.Entry(user)
                    .Reference(x => x.department)
                    .LoadAsync();

                await _context.Entry(user)
                    .Reference(x => x.role)
                    .LoadAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"Repository Error: Failed to update user. Error: {ex.Message}");

                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                _logger.LogInformation($"Repository: Deleting user with ID {userId} from database.");

                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning($"Repository: User with ID {userId} not found for deletion.");
                    return false;
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Repository: Successfully deleted user with ID {userId}.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository Error: Failed to delete user with ID {userId}. Error: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> UserExistsAsync(int userId)
        {
            try
            {
                return await _context.Users
                    .AsNoTracking()
                    .AnyAsync(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository Error: Failed to check if user exists. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<PagedResult<Patient>>GetAllPatientsAsync(
        int page,
        int pageSize,
        string? search,
        string? status)
        {
            var query =
                _context.Patients
                    .Include(x => x.Department)
                    .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.FirstName.Contains(search)
                    ||
                    x.LastName.Contains(search)
                    ||
                    x.Mrn.Contains(search));
            }

            if (status == "active")
            {
                query =
                    query.Where(
                        x => x.IsActive == 1);
            }

            if (status == "discharged")
            {
                query =
                    query.Where(
                        x => x.IsActive == 0);
            }

            var totalCount =
                await query.CountAsync();

            var patients =
                await query
                    .OrderBy(x => x.FirstName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

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

        public async Task<Patient?> GetPatientByIdAsync(
    int patientId)
        {
            try
            {
                _logger.LogInformation(
                    $"Repository: Retrieving patient with ID {patientId}");

                if (patientId <= 0)
                {
                    return null;
                }

                _context.ChangeTracker.Clear();

                var patient =
                    await _context.Patients
                        .Include(p => p.Department)
                        .FirstOrDefaultAsync(
                            p => p.PatientId == patientId);

                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"Error retrieving patient {patientId}");

                throw;
            }
        }

        public async Task<bool> DepartmentExistsAsync(byte deptId)
        {
            try
            {
                return await _context.Departments
                    .AsNoTracking()
                    .AnyAsync(d => d.DeptId == deptId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository Error: Failed to check if department exists. Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> RoleExistsAsync(byte roleId)
        {
            try
            {
                return await _context.Roles
                    .AsNoTracking()
                    .AnyAsync(r => r.RoleId == roleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository Error: Failed to check if role exists. Error: {ex.Message}");
                throw;
            }
        }
        public async Task<User?>GetTrackedUserByIdAsync(
        int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    x => x.UserId == userId);
        }
    }
}