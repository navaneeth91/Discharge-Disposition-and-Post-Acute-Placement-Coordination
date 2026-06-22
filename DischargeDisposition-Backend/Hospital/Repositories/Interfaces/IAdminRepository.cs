using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for administrative data access operations.
    /// </summary>
    public interface IAdminRepository
    {
        /// <summary>
        /// Retrieves all users with their related department and role information.
        /// </summary>
        /// <returns>A collection of users.</returns>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves a specific user by ID with related information.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user if found; otherwise null.</returns>
        Task<User?> GetUserByIdAsync(int userId);

        /// <summary>
        /// Updates a user in the database.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        /// <returns>The updated user.</returns>
        Task<User> UpdateUserAsync(User user);

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>True if deletion was successful; otherwise false.</returns>
        Task<bool> DeleteUserAsync(int userId);

        /// <summary>
        /// Checks if a user exists by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <returns>True if user exists; otherwise false.</returns>
        Task<bool> UserExistsAsync(int userId);

        /// <summary>
        /// Retrieves all active patients with their related department information.
        /// </summary>
        /// <returns>A collection of active patients.</returns>
        Task<IEnumerable<Patient>> GetAllPatientsAsync();

        /// <summary>
        /// Retrieves a specific patient by ID with related information.
        /// </summary>
        /// <param name="patientId">The ID of the patient to retrieve.</param>
        /// <returns>The patient if found; otherwise null.</returns>
        Task<Patient?> GetPatientByIdAsync(int patientId);

        /// <summary>
        /// Checks if a department exists by ID.
        /// </summary>
        /// <param name="deptId">The ID of the department to check.</param>
        /// <returns>True if department exists; otherwise false.</returns>
        Task<bool> DepartmentExistsAsync(byte deptId);

        /// <summary>
        /// Checks if a role exists by ID.
        /// </summary>
        /// <param name="roleId">The ID of the role to check.</param>
        /// <returns>True if role exists; otherwise false.</returns>
        Task<bool> RoleExistsAsync(byte roleId);
    }
}