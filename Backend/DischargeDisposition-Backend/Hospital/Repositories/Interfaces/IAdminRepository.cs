using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<PagedResult<User>>GetAllUsersAsync(
        int page,
        int pageSize,
        string? search);

        Task<PagedResult<Patient>> GetAllPatientsAsync(
            int page,
            int pageSize,
            string? search,
            string? status);

        Task<User?> GetUserByIdAsync(int userId);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UserExistsAsync(int userId);
        Task<Patient?> GetPatientByIdAsync(int patientId);
        Task<bool> DepartmentExistsAsync(byte deptId);
        Task<bool> RoleExistsAsync(byte roleId);
        Task<User?> GetTrackedUserByIdAsync(int userId);
    }
}