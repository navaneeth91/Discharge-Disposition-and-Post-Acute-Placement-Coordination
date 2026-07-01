using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetUsersByRoleAsync(string roleName);
        Task<List<User>> GetAdministratorsAsync();
        Task AddAsync(User user);
    }
}