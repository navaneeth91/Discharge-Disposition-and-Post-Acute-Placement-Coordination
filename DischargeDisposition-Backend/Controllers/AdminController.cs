using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DischargeDisposition_Backend.Controllers
{
    /// <summary>
    /// Administrative controller for managing users and patients.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        /// <summary>
        /// Initializes a new instance of the AdminController class.
        /// </summary>
        /// <param name="adminService">The admin service instance.</param>
        /// <param name="logger">The logger instance.</param>
        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        /// <response code="200">Returns the list of users.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("GET /api/admin/users - Retrieving all users.");
                var users = await _adminService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving users." });
            }
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user details.</returns>
        /// <response code="200">Returns the user details.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("users/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            try
            {
                _logger.LogInformation("GET /api/admin/users/{UserId} - Retrieving user by ID.", id);

                var user = await _adminService.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", id);
                    return NotFound(new { message = $"User with ID {id} not found." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {UserId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the user." });
            }
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="updateUserDto">The updated user information.</param>
        /// <returns>The updated user details.</returns>
        /// <response code="200">Returns the updated user details.</response>
        /// <response code="400">Bad request - validation failed.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut("users/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                if (updateUserDto == null)
                {
                    return BadRequest(new { message = "Request body cannot be empty." });
                }

                _logger.LogInformation("PUT /api/admin/users/{UserId} - Updating user.", id);

                var updatedUser = await _adminService.UpdateUserAsync(id, updateUserDto);
                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error updating user with ID {UserId}.", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the user." });
            }
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>Success response.</returns>
        /// <response code="204">User successfully deleted.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("users/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation("DELETE /api/admin/users/{UserId} - Deleting user.", id);

                await _adminService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "User with ID {UserId} not found for deletion.", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the user." });
            }
        }

        /// <summary>
        /// Retrieves all patients.
        /// </summary>
        /// <returns>A list of all active patients.</returns>
        /// <response code="200">Returns the list of patients.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("patients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
        {
            try
            {
                _logger.LogInformation("GET /api/admin/patients - Retrieving all patients.");
                var patients = await _adminService.GetAllPatientsAsync();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patients.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving patients." });
            }
        }

        /// <summary>
        /// Retrieves a patient by ID.
        /// </summary>
        /// <param name="id">The patient ID.</param>
        /// <returns>The patient details.</returns>
        /// <response code="200">Returns the patient details.</response>
        /// <response code="404">Patient not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("patients/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PatientDto>> GetPatientById(int id)
        {
            try
            {
                _logger.LogInformation("GET /api/admin/patients/{PatientId} - Retrieving patient by ID.", id);

                var patient = await _adminService.GetPatientByIdAsync(id);

                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found.", id);
                    return NotFound(new { message = $"Patient with ID {id} not found." });
                }

                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with ID {PatientId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the patient." });
            }
        }
    }
}