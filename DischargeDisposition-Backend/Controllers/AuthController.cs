using DischargeDisposition_Backend.DTOs.Requests;
using DischargeDisposition_Backend.Helpers;
using Microsoft.AspNetCore.Mvc;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.DTOs.Responses;


namespace DischargeDisposition_Backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(
            SignupRequest request)
        {
            var response =
                await _authService.SignupAsync(request);

            return this.ToHttpResponse(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginRequest request)
        {
            var token = await _authService.LoginAsync(request);
            if (token == null)
            {
                return this.ToHttpResponse(new ApiResponse<LoginResponse>
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Login failed",
                    Errors = new List<string>
                    {
                        "Invalid email or password"
                    }
                });
            }
            return this.ToHttpResponse(new ApiResponse<LoginResponse>
            {
                Success = true,
                StatusCode = 200,
                Message = "Login successful",
                Data = new LoginResponse{ Token = token }
            });
        }
    }
}