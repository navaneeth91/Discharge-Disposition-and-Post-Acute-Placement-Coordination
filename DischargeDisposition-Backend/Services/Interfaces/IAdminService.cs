using DischargeDisposition_Backend.Hospital.DTOs;

namespace DischargeDisposition_Backend.Services.Interfaces
{
    /// <summary>
    /// Service interface for administrative operations on users and patients.
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// Retrieves all users with their department and role information.
        /// </summary>
        /// <returns>A collection of user DTOs.</returns>
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves a specific user by ID with related information.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user DTO if found; otherwise null.</returns>
        Task<UserDto?> GetUserByIdAsync(int userId);

        /// <summary>
        /// Updates user details.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="updateUserDto">The updated user information.</param>
        /// <returns>The updated user DTO.</returns>
        /// <exception cref="ArgumentException">Thrown when user is not found or validation fails.</exception>
        Task<UserDto> UpdateUserAsync(int userId, UpdateUserDto updateUserDto);

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <exception cref="ArgumentException">Thrown when user is not found.</exception>
        Task DeleteUserAsync(int userId);

        /// <summary>
        /// Retrieves all patients.
        /// </summary>
        /// <returns>A collection of patient DTOs.</returns>
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();

        /// <summary>
        /// Retrieves a specific patient by ID.
        /// </summary>
        /// <param name="patientId">The ID of the patient to retrieve.</param>
        /// <returns>The patient DTO if found; otherwise null.</returns>
        Task<PatientDto?> GetPatientByIdAsync(int patientId);
    }
}