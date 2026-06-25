using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{

    public interface IAdminRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UserExistsAsync(int userId);
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient?> GetPatientByIdAsync(int patientId);
        Task<bool> DepartmentExistsAsync(byte deptId);
        Task<bool> RoleExistsAsync(byte roleId);
    }
}