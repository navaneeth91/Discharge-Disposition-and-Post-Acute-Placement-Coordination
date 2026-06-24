using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IAdminService
    {
        Task<ApiResponse<IEnumerable<UserDto>>>GetAllUsersAsync();

        Task<ApiResponse<UserDto>>GetUserByIdAsync(int userId);

        Task<ApiResponse<UserDto>>UpdateUserAsync(int userId,UpdateUserDto updateUserRequest);

        Task<ApiResponse<object>>DeleteUserAsync(int userId);

        Task<ApiResponse<IEnumerable<PatientDto>>>GetAllPatientsAsync();

        Task<ApiResponse<PatientDto>>GetPatientByIdAsync(int patientId);
    }
}