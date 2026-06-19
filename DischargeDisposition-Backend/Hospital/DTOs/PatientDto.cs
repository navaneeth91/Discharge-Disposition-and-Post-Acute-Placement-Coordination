namespace DischargeDisposition_Backend.Hospital.DTOs
{
    /// <summary>
    /// Data Transfer Object for patient information.
    /// </summary>
    public class PatientDto
    {
        /// <summary>Gets or sets the patient ID.</summary>
        public int PatientId { get; set; }

        /// <summary>Gets or sets the Medical Record Number.</summary>
        public string Mrn { get; set; } = string.Empty;

        /// <summary>Gets or sets the first name.</summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Gets or sets the last name.</summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>Gets or sets the date of birth.</summary>
        public DateOnly DateOfBirth { get; set; }

        /// <summary>Gets or sets the admission date.</summary>
        public DateTime AdmissionDate { get; set; }

        /// <summary>Gets or sets the expected discharge date.</summary>
        public DateOnly ExpectedDischargeDate { get; set; }

        /// <summary>Gets or sets the actual discharge date.</summary>
        public DateTime? ActualDischargeDate { get; set; }

        /// <summary>Gets or sets the gender.</summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>Gets or sets the email address.</summary>
        public string? Email { get; set; }

        /// <summary>Gets or sets the phone number.</summary>
        public string? PhoneNumber { get; set; }

        /// <summary>Gets or sets the department ID.</summary>
        public byte DeptId { get; set; }

        /// <summary>Gets or sets the department name.</summary>
        public string? DepartmentName { get; set; }
    }
}