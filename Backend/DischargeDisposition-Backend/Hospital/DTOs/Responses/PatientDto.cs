namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string Mrn { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateOnly ExpectedDischargeDate { get; set; }
        public DateTime? ActualDischargeDate { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public byte DeptId { get; set; }
        public string? DepartmentName { get; set; }
        public byte IsActive { get; set; }
    }
}