using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class PatientAssignmentService : IPatientAssignmentService
    {
        private readonly IPatientAssignmentRepository _repository;
        private readonly IAdminRepository _adminRepository;
        private readonly ILogger<PatientAssignmentService> _logger;

        public PatientAssignmentService(
            IPatientAssignmentRepository repository,
            IAdminRepository adminRepository,
            ILogger<PatientAssignmentService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _adminRepository = adminRepository ?? throw new ArgumentNullException(nameof(adminRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<PatientAssignmentDto>> AssignCareManagerAsync(
            AssignCareManagerRequest request)
        {
            try
            {
                var patient =
                    await _adminRepository.GetPatientByIdAsync(request.PatientId);

                if (patient == null)
                {
                    return new ApiResponse<PatientAssignmentDto>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "Patient not found."
                    };
                }

                var careManagerExists =
                    await _repository.CareManagerExistsAsync(request.CareManagerId);

                if (!careManagerExists)
                {
                    return new ApiResponse<PatientAssignmentDto>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "Care Manager not found."
                    };
                }

                var alreadyAssigned =
                    await _repository.IsPatientAssignedAsync(request.PatientId);

                if (alreadyAssigned)
                {
                    return new ApiResponse<PatientAssignmentDto>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "Patient is already assigned to a Care Manager."
                    };
                }

                var assignment = new PatientAssignment
                {
                    PatientId = request.PatientId,
                    CareManagerId = request.CareManagerId,
                    AssignedBy = request.AssignedBy,
                    AssignedDate = DateTime.UtcNow,
                    IsActive = true,
                    Notes = request.Notes
                };

                var createdAssignment =
                    await _repository.AssignCareManagerAsync(assignment);

                var careManager =
                    (await _repository.GetAllCareManagersAsync())
                    .FirstOrDefault(c => c.UserId == request.CareManagerId);

                return new ApiResponse<PatientAssignmentDto>
                {
                    Success = true,
                    StatusCode = 201,
                    Message = "Care Manager assigned successfully.",
                    Data = MapAssignmentToDto(
                        createdAssignment,
                        patient,
                        careManager!)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error assigning Care Manager.");

                return new ApiResponse<PatientAssignmentDto>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to assign Care Manager.",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<PagedResult<PatientDto>>> GetUnassignedPatientsAsync(
            int page,
            int pageSize,
            string? search)
        {
            try
            {
                var result =
                    await _repository
                        .GetUnassignedPatientsAsync(
                            page,
                            pageSize,
                            search);

                var pagedPatients =
                    new PagedResult<PatientDto>
                    {
                        Items = result.Items
                            .Select(MapPatientToDto)
                            .ToList(),

                        Page = result.Page,

                        PageSize = result.PageSize,

                        TotalCount = result.TotalCount,

                        TotalPages = result.TotalPages
                    };

                return new ApiResponse<PagedResult<PatientDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Unassigned patients retrieved successfully.",

                    Data = pagedPatients
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving unassigned patients.");

                return new ApiResponse<PagedResult<PatientDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve unassigned patients.",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
        public async Task<ApiResponse<IEnumerable<PatientAssignmentDto>>> GetPatientsByCareManagerAsync(
    int careManagerId)
        {
            try
            {
                var assignments =
                    await _repository.GetPatientsByCareManagerAsync(careManagerId);

                return new ApiResponse<IEnumerable<PatientAssignmentDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Assigned patients retrieved successfully.",
                    Data = assignments.Select(a =>
                        MapAssignmentToDto(
                            a,
                            a.Patient,
                            a.CareManager))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving assigned patients for Care Manager {CareManagerId}.",
                    careManagerId);

                return new ApiResponse<IEnumerable<PatientAssignmentDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve assigned patients.",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllCareManagersAsync()
        {
            try
            {
                var careManagers =
                    await _repository.GetAllCareManagersAsync();

                return new ApiResponse<IEnumerable<UserDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Care Managers retrieved successfully.",
                    Data = careManagers.Select(MapUserToDto)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving Care Managers.");

                return new ApiResponse<IEnumerable<UserDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve Care Managers.",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        private static PatientAssignmentDto MapAssignmentToDto(
            PatientAssignment assignment,
            Patient patient,
            User careManager)
        {
            return new PatientAssignmentDto
            {
                AssignmentId = assignment.AssignmentId,
                PatientId = patient.PatientId,
                PatientName = $"{patient.FirstName} {patient.LastName}",
                CareManagerId = careManager.UserId,
                CareManagerName = $"{careManager.FirstName} {careManager.LastName}",
                AssignedDate = assignment.AssignedDate,
                IsActive = assignment.IsActive
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
                DepartmentName = patient.Department?.Name
            };
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
    }
}