namespace DischargeDisposition_Backend.DTOs.Responses
{
    public class DispositionDecisionResponse
    {
        public int DecisionId { get; set; }

        public int PatientId { get; set; }

        public int DispositionTypeId { get; set; }

        public int ClinicianId { get; set; }

        public byte DepartmentId { get; set; }

        public DateTime DecisionDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateOnly ExpectedTransitionDate { get; set; }
    }
}