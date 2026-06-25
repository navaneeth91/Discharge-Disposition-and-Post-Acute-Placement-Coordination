using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IAdminService
    {
        Task<ApiResponse<PagedResult<UserDto>>>GetAllUsersAsync(
        int page,
        int pageSize,
        string? search);

        Task<ApiResponse<PagedResult<PatientDto>>>GetAllPatientsAsync(
        int page,
        int pageSize,
        string? search,
        string? status);

        Task<ApiResponse<UserDto>>GetUserByIdAsync(int userId);

        Task<ApiResponse<UserDto>>UpdateUserAsync(int userId,UpdateUserDto updateUserRequest);

        Task<ApiResponse<object>>DeleteUserAsync(int userId);

        Task<ApiResponse<PatientDto>>GetPatientByIdAsync(int patientId);
    }
}