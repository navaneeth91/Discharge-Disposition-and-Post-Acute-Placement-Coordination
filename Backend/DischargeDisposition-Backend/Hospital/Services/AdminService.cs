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

        public async Task<ApiResponse<IEnumerable<UserDto>>>GetAllUsersAsync()
        {
            try
            {
                var users =await _repository
                        .GetAllUsersAsync();

                return new ApiResponse<IEnumerable<UserDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Users retrieved successfully",
                    Data = users.Select(MapUserToDto)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error retrieving users");

                return new ApiResponse<IEnumerable<UserDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve users",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<UserDto>>GetUserByIdAsync(int userId)
        {
            try
            {
                var user =await _repository
                        .GetUserByIdAsync(userId);

                if (user == null)
                {
                    return new ApiResponse<UserDto>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "User not found"
                    };
                }

                return new ApiResponse<UserDto>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "User retrieved successfully",
                    Data = MapUserToDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error retrieving user");

                return new ApiResponse<UserDto>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve user",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<UserDto>> UpdateUserAsync(int userId,UpdateUserDto updateUserDto)
        {
            try
            {
                var user =await _repository.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return new ApiResponse<UserDto>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = $"User with ID {userId} not found"
                    };
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

                var updatedUser = await _repository.UpdateUserAsync(user);

                return new ApiResponse<UserDto>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "User updated successfully",
                    Data = MapUserToDto(updatedUser)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating user with ID {UserId}",
                    userId);

                return new ApiResponse<UserDto>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to update user",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<object>>DeleteUserAsync(int userId)
        {
            try
            {
                var user =
                    await _repository.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "User not found"
                    };
                }

                await _repository
                    .DeleteUserAsync(userId);

                return new ApiResponse<object>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "User deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to delete user",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<PatientDto>>>GetAllPatientsAsync()
        {
            try
            {
                var patients =
                    await _repository.GetAllPatientsAsync();

                return new ApiResponse<IEnumerable<PatientDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Patients retrieved successfully",
                    Data = patients.Select(MapPatientToDto)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving patients");

                return new ApiResponse<IEnumerable<PatientDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve patients",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<PatientDto>>GetPatientByIdAsync(int patientId)
        {
            try
            {
                var patient =await _repository.GetPatientByIdAsync(patientId);

                if (patient == null)
                {
                    return new ApiResponse<PatientDto>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "Patient not found"
                    };
                }

                return new ApiResponse<PatientDto>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Patient retrieved successfully",
                    Data = MapPatientToDto(patient)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving patient with ID {PatientId}",
                    patientId);

                return new ApiResponse<PatientDto>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve patient",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
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
