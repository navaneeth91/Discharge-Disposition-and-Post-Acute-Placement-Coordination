using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(
            IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult>
            GetUsers()
        {
            var response =
                await _adminService
                    .GetAllUsersAsync();

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult>
            GetUserById(int id)
        {
            var response =
                await _adminService
                    .GetUserByIdAsync(id);

            return this
                .ToHttpResponse(response);
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult>
            UpdateUser(
                int id,
                [FromBody]
                UpdateUserDto updateUserDto)
        {
            var response =
                await _adminService
                    .UpdateUserAsync(
                        id,
                        updateUserDto);

            return this
                .ToHttpResponse(response);
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult>
            DeleteUser(int id)
        {
            var response =
                await _adminService
                    .DeleteUserAsync(id);

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("patients")]
        public async Task<IActionResult>
            GetPatients()
        {
            var response =
                await _adminService
                    .GetAllPatientsAsync();

            return this
                .ToHttpResponse(response);
        }

        [HttpGet("patients/{id}")]
        public async Task<IActionResult>
            GetPatientById(int id)
        {
            var response =
                await _adminService
                    .GetPatientByIdAsync(id);

            return this
                .ToHttpResponse(response);
        }
    }
}