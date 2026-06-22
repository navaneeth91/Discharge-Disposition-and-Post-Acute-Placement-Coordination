using System.ComponentModel.DataAnnotations;

namespace DischargeDisposition_Backend.Hospital.DTOs.Request
{
    /// <summary>
    /// Data Transfer Object for updating user information.
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>Gets or sets the username.</summary>
        [MaxLength(20)]
        public string? UserName { get; set; }

        /// <summary>Gets or sets the first name.</summary>
        [MaxLength(50)]
        public string? FirstName { get; set; }

        /// <summary>Gets or sets the last name.</summary>
        [MaxLength(50)]
        public string? LastName { get; set; }

        /// <summary>Gets or sets the phone number.</summary>
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        /// <summary>Gets or sets the email address.</summary>
        [MaxLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>Gets or sets the department ID.</summary>
        public byte? DeptId { get; set; }

        /// <summary>Gets or sets the role ID.</summary>
        public byte? RoleId { get; set; }
    }
}