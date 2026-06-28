namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
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