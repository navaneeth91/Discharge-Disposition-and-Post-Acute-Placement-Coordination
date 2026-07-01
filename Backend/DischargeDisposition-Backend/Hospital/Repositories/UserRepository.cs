using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HospitalDbContext _context;

        public UserRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.role)
                .FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.role)
                .SingleOrDefaultAsync(u => u.UserId == id);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetUsersByRoleAsync(string roleName)
        {
            return await _context.Users
                .Include(u => u.role)
                .Where(u => u.role.Name == roleName)
                .ToListAsync();
        }
        public async Task<List<User>> GetAdministratorsAsync()
        {
            return await _context.Users

                .Include(u => u.role)

                .Where(u =>
                    u.role.Name == "Administrator")

                .ToListAsync();
        }
    }
}