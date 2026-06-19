using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Services
{
    /// <summary>
    /// Service for administrative operations on users and patients.
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly HospitalDbContext _context;
        private readonly ILogger<AdminService> _logger;

        /// <summary>
        /// Initializes a new instance of the AdminService class.
        /// </summary>
        /// <param name="context">The hospital database context.</param>
        /// <param name="logger">The logger instance.</param>
        public AdminService(HospitalDbContext context, ILogger<AdminService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves all users with their department and role information.
        /// </summary>
        /// <returns>A collection of user DTOs.</returns>
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all users.");

                var users = await _context.Users
                    .Include(u => u.department)
                    .Include(u => u.role)
                    .ToListAsync();

                var userDtos = users.Select(MapUserToDto).ToList();

                _logger.LogInformation("Successfully retrieved {UserCount} users.", userDtos.Count);

                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a specific user by ID with related information.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user DTO if found; otherwise null.</returns>
        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Retrieving user with ID {UserId}.", userId);

                var user = await _context.Users
                    .Include(u => u.department)
                    .Include(u => u.role)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", userId);
                    return null;
                }

                _logger.LogInformation("Successfully retrieved user with ID {UserId}.", userId);

                return MapUserToDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {UserId}.", userId);
                throw;
            }
        }

        /// <summary>
        /// Updates user details.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="updateUserDto">The updated user information.</param>
        /// <returns>The updated user DTO.</returns>
        /// <exception cref="ArgumentException">Thrown when user is not found or validation fails.</exception>
        public async Task<UserDto> UpdateUserAsync(int userId, UpdateUserDto updateUserDto)
        {
            try
            {
                if (updateUserDto == null)
                {
                    throw new ArgumentException("Update user data cannot be null.", nameof(updateUserDto));
                }

                _logger.LogInformation("Updating user with ID {UserId}.", userId);

                var user = await _context.Users
                    .Include(u => u.department)
                    .Include(u => u.role)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found for update.", userId);
                    throw new ArgumentException($"User with ID {userId} not found.", nameof(userId));
                }

                // Validate department exists if being updated
                if (updateUserDto.DeptId.HasValue && updateUserDto.DeptId != user.DeptId)
                {
                    var deptExists = await _context.Departments
                        .AnyAsync(d => d.DeptId == updateUserDto.DeptId);

                    if (!deptExists)
                    {
                        throw new ArgumentException($"Department with ID {updateUserDto.DeptId} not found.", nameof(updateUserDto.DeptId));
                    }
                }

                // Validate role exists if being updated
                if (updateUserDto.RoleId.HasValue && updateUserDto.RoleId != user.RoleId)
                {
                    var roleExists = await _context.Roles
                        .AnyAsync(r => r.RoleId == updateUserDto.RoleId);

                    if (!roleExists)
                    {
                        throw new ArgumentException($"Role with ID {updateUserDto.RoleId} not found.", nameof(updateUserDto.RoleId));
                    }
                }

                // Update user properties
                if (!string.IsNullOrWhiteSpace(updateUserDto.UserName))
                {
                    user.UserName = updateUserDto.UserName;
                }

                if (!string.IsNullOrWhiteSpace(updateUserDto.FirstName))
                {
                    user.FirstName = updateUserDto.FirstName;
                }

                if (!string.IsNullOrWhiteSpace(updateUserDto.LastName))
                {
                    user.LastName = updateUserDto.LastName;
                }

                if (!string.IsNullOrWhiteSpace(updateUserDto.PhoneNumber))
                {
                    user.PhoneNumber = updateUserDto.PhoneNumber;
                }

                if (!string.IsNullOrWhiteSpace(updateUserDto.Email))
                {
                    user.Email = updateUserDto.Email;
                }

                if (updateUserDto.DeptId.HasValue)
                {
                    user.DeptId = updateUserDto.DeptId.Value;
                }

                if (updateUserDto.RoleId.HasValue)
                {
                    user.RoleId = updateUserDto.RoleId.Value;
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated user with ID {UserId}.", userId);

                // Reload navigation properties for DTO mapping
                await _context.Entry(user).Reference(u => u.department).LoadAsync();
                await _context.Entry(user).Reference(u => u.role).LoadAsync();

                return MapUserToDto(user);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}.", userId);
                throw;
            }
        }

        /// <summary>
        /// Deletes a user by ID (hard delete).
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <exception cref="ArgumentException">Thrown when user is not found.</exception>
        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID {UserId}.", userId);

                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found for deletion.", userId);
                    throw new ArgumentException($"User with ID {userId} not found.", nameof(userId));
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted user with ID {UserId}.", userId);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}.", userId);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all patients.
        /// </summary>
        /// <returns>A collection of patient DTOs.</returns>
        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all patients.");

                var patients = await _context.Patients
                    .Include(p => p.department)
                    .Where(p => p.IsActive == 1)
                    .ToListAsync();

                var patientDtos = patients.Select(MapPatientToDto).ToList();

                _logger.LogInformation("Successfully retrieved {PatientCount} patients.", patientDtos.Count);

                return patientDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all patients.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a specific patient by ID.
        /// </summary>
        /// <param name="patientId">The ID of the patient to retrieve.</param>
        /// <returns>The patient DTO if found; otherwise null.</returns>
        public async Task<PatientDto?> GetPatientByIdAsync(int patientId)
        {
            try
            {
                _logger.LogInformation("Retrieving patient with ID {PatientId}.", patientId);

                var patient = await _context.Patients
                    .Include(p => p.department)
                    .FirstOrDefaultAsync(p => p.PatientId == patientId && p.IsActive == 1);

                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found.", patientId);
                    return null;
                }

                _logger.LogInformation("Successfully retrieved patient with ID {PatientId}.", patientId);

                return MapPatientToDto(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with ID {PatientId}.", patientId);
                throw;
            }
        }

        /// <summary>
        /// Maps a User entity to a UserDto.
        /// </summary>
        private static UserDto MapUserToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                DeptId = user.DeptId,
                DepartmentName = user.department?.Name,
                RoleId = user.RoleId,
                RoleName = user.role?.Name,
                CreatedAt = user.CreatedAt
            };
        }

        /// <summary>
        /// Maps a Patient entity to a PatientDto.
        /// </summary>
        private static PatientDto MapPatientToDto(Patient patient)
        {
            return new PatientDto
            {
                PatientId = patient.PatientId,
                Mrn = patient.Mrn,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.Dob,
                AdmissionDate = patient.AdmissionDate,
                ExpectedDischargeDate = patient.ExpectedDischargeDate,
                ActualDischargeDate = patient.ActualDischargeDate,
                Gender = patient.Gender.ToString(),
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DeptId = patient.DeptId,
                DepartmentName = patient.department?.Name
            };
        }
    }
}