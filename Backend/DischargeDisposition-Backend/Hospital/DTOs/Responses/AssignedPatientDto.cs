namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class AssignedPatientDto
    {
        public int PatientId { get; set; }

        public string Mrn { get; set; } = string.Empty;

        public string PatientName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public DateOnly? ExpectedDischargeDate { get; set; }

        public int? DispositionTypeId { get; set; }

        public string? DispositionType { get; set; }

        public bool HasReferral { get; set; }

        public string? ReferralStatus { get; set; }
    }
}