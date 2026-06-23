namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class PatientDelayResponse
    {
        public int PatientDelayId { get; set; }

        public int PatientId { get; set; }

        public byte DelayReasonId { get; set; }

        public int ReportedBy { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}