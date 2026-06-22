namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class DispositionDecisionDetailsResponse
    {
        public int DecisionId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public int DispositionTypeId { get; set; }

        public string DispositionTypeName { get; set; } = string.Empty;

        public int ClinicianId { get; set; }

        public string ClinicianName { get; set; } = string.Empty;

        public byte DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public DateTime DecisionDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateOnly ExpectedTransitionDate { get; set; }
    }
}