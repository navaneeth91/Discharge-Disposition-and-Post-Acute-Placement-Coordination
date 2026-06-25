using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
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
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Repository: Retrieving all users from database.");

                var users = await _context.Users
                    .Include(u => u.department)
                    .Include(u => u.role)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation($"Repository: Successfully retrieved {users.Count} users.");

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository Error: Failed to retrieve all users. Error: {ex.Message}");
                throw;
            }
        }
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

        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User entity cannot be null.");
                }

                _logger.LogInformation($"Repository: Updating user with ID {user.UserId} in database.");

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Repository: Successfully updated user with ID {user.UserId}.");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository Error: Failed to update user. Error: {ex.Message}");
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
        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            try
            {
                _logger.LogInformation("Repository: Retrieving all active patients from database.");

                var patients = await _context.Patients
                    .Include(p => p.Department)
                    .Where(p => p.IsActive == 1)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation($"Repository: Successfully retrieved {patients.Count} patients.");

                return patients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository Error: Failed to retrieve all patients. Error: {ex.Message}");
                throw;
            }
        }
        public async Task<Patient?> GetPatientByIdAsync(int patientId)
        {
            try
            {
                _logger.LogInformation($"Repository: Retrieving patient with ID {patientId} from database.");

                if (patientId <= 0)
                {
                    _logger.LogWarning($"Repository: Invalid patient ID provided: {patientId}");
                    return null;
                }

                var patient = await _context.Patients
                    .Include(p => p.Department)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PatientId == patientId && p.IsActive == 1);

                if (patient == null)
                {
                    _logger.LogWarning($"Repository: Patient with ID {patientId} not found in database.");
                }
                else
                {
                    _logger.LogInformation($"Repository: Successfully retrieved patient with ID {patientId}.");
                }

                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository Error: Failed to retrieve patient with ID {patientId}. Error: {ex.Message}");
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
    }
}