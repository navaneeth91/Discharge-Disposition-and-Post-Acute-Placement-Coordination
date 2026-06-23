using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;
        private readonly ILogger<AdminService> _logger;

        public AdminService(
            IAdminRepository repository,
            ILogger<AdminService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all users");

                var users = await _repository.GetAllUsersAsync();

                return users.Select(MapUserToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                throw;
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Retrieving user with ID {UserId}", userId);

                var user = await _repository.GetUserByIdAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", userId);
                    return null;
                }

                return MapUserToDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {UserId}", userId);
                throw;
            }
        }

        public async Task<UserDto> UpdateUserAsync(
            int userId,
            UpdateUserDto updateUserDto)
        {
            try
            {
                _logger.LogInformation(
                    "Updating user with ID {UserId}",
                    userId);

                var user = await _repository.GetUserByIdAsync(userId);

                if (user == null)
                {
                    throw new ArgumentException(
                        $"User with ID {userId} not found.");
                }

                if (!string.IsNullOrWhiteSpace(updateUserDto.UserName))
                    user.UserName = updateUserDto.UserName;

                if (!string.IsNullOrWhiteSpace(updateUserDto.FirstName))
                    user.FirstName = updateUserDto.FirstName;

                if (!string.IsNullOrWhiteSpace(updateUserDto.LastName))
                    user.LastName = updateUserDto.LastName;

                if (!string.IsNullOrWhiteSpace(updateUserDto.Email))
                    user.Email = updateUserDto.Email;

                if (!string.IsNullOrWhiteSpace(updateUserDto.PhoneNumber))
                    user.PhoneNumber = updateUserDto.PhoneNumber;

                if (updateUserDto.DeptId.HasValue)
                    user.DeptId = updateUserDto.DeptId.Value;

                if (updateUserDto.RoleId.HasValue)
                    user.RoleId = updateUserDto.RoleId.Value;

                var updatedUser =
                    await _repository.UpdateUserAsync(user);

                _logger.LogInformation(
                    "User updated successfully with ID {UserId}",
                    userId);

                return MapUserToDto(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating user with ID {UserId}",
                    userId);

                throw;
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                _logger.LogInformation(
                    "Deleting user with ID {UserId}",
                    userId);

                var user = await _repository.GetUserByIdAsync(userId);

                if (user == null)
                {
                    throw new ArgumentException(
                        $"User with ID {userId} not found.");
                }

                await _repository.DeleteUserAsync(userId);

                _logger.LogInformation(
                    "User deleted successfully with ID {UserId}",
                    userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error deleting user with ID {UserId}",
                    userId);

                throw;
            }
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all patients");

                var patients =
                    await _repository.GetAllPatientsAsync();

                return patients.Select(MapPatientToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patients");
                throw;
            }
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int patientId)
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving patient with ID {PatientId}",
                    patientId);

                var patient =
                    await _repository.GetPatientByIdAsync(patientId);

                if (patient == null)
                {
                    _logger.LogWarning(
                        "Patient with ID {PatientId} not found",
                        patientId);

                    return null;
                }

                return MapPatientToDto(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving patient with ID {PatientId}",
                    patientId);

                throw;
            }
        }

        private static UserDto MapUserToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DeptId = user.DeptId,
                DepartmentName = user.department?.Name,
                RoleId = user.RoleId,
                RoleName = user.role?.Name,
                CreatedAt = user.CreatedAt
            };
        }

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
