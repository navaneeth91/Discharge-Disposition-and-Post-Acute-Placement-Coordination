namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class PatientDelayDetailsResponse
    {
        public int PatientDelayId { get; set; }

        public int PatientId { get; set; }

        public byte DelayReasonId { get; set; }

        public string DelayReason { get; set; } = string.Empty;

        public int ReportedBy { get; set; }

        public string ReportedByName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}