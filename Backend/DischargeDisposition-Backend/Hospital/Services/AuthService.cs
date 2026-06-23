using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwt;
        private readonly HospitalDbContext _hospitalDbContext;

        public AuthService(
            IUserRepository userRepository,
            HospitalDbContext hospitalDbContext,
            JwtHelper jwt)
        {
            _userRepository = userRepository;
            _hospitalDbContext = hospitalDbContext;
            _jwt = jwt;
        }

        public async Task<ApiResponse<object>> SignupAsync(SignupRequest request)
        {
            try
            {
                var existingUser =
                    await _userRepository.GetByEmailAsync(request.Email);

                if (existingUser != null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 409,
                        Message = "Signup failed",
                        Errors = new List<string>
                        {
                            "Email already exists"
                        }
                    };
                }

                var isFirstUser =
                    !await _hospitalDbContext.Users.AnyAsync();

                var roleName =
                    isFirstUser ? "Administrator" : "Unassigned";

                var role = await _hospitalDbContext.Roles
                    .SingleOrDefaultAsync(r => r.Name == roleName);

                if (role == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 500,
                        Message = "Role configuration error",
                        Errors = new List<string>
                        {
                            $"{roleName} role not found"
                        }
                    };
                }

                var unassignedDepartment =
                    await _hospitalDbContext.Departments
                        .SingleOrDefaultAsync(d => d.Name == "UNASSIGNED");

                if (unassignedDepartment == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 500,
                        Message = "Department configuration error",
                        Errors = new List<string>
                        {
                            "UNASSIGNED department not found"
                        }
                    };
                }

                var user = new User
                {
                    UserName = request.UserName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash =
                        PasswordHasher.HashPassword(request.Password),

                    RoleId = role.RoleId,
                    DeptId = unassignedDepartment.DeptId,

                };

                await _userRepository.AddAsync(user);

                return new ApiResponse<object>
                {
                    Success = true,
                    StatusCode = 201,
                    Message = "User registered successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Signup failed",
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }
        }
        public async Task<string?> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return null;
            }
            var isPasswordValid =
                PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return null;
            }
            var token = _jwt.GenerateToken(
                        user.UserId,
                        user.UserName,
                        user.role?.Name ?? "UNASSIGNED",
                        null,
                        null);
            return token;


        }
    }
}