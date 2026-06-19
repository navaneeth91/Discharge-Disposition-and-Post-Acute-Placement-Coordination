namespace DischargeDisposition_Backend.Hospital.DTOs
{
    /// <summary>
    /// Data Transfer Object for user information.
    /// </summary>
    public class UserDto
    {
        /// <summary>Gets or sets the user ID.</summary>
        public int UserId { get; set; }

        /// <summary>Gets or sets the username.</summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>Gets or sets the first name.</summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Gets or sets the last name.</summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>Gets or sets the phone number.</summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>Gets or sets the email address.</summary>
        public string? Email { get; set; }

        /// <summary>Gets or sets the department ID.</summary>
        public byte DeptId { get; set; }

        /// <summary>Gets or sets the department name.</summary>
        public string? DepartmentName { get; set; }

        /// <summary>Gets or sets the role ID.</summary>
        public byte RoleId { get; set; }

        /// <summary>Gets or sets the role name.</summary>
        public string? RoleName { get; set; }

        /// <summary>Gets or sets the creation date.</summary>
        public DateTime CreatedAt { get; set; }
    }
}