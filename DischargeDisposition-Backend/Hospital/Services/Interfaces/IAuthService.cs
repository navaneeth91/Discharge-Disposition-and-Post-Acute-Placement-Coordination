using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> SignupAsync(SignupRequest request);
        Task<string?> LoginAsync(LoginRequest request);
    }
}