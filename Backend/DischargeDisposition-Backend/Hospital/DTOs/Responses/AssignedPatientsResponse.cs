namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class AssignedPatientsResponse
    {
        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;
        public DateOnly Dob { get; set; }
        public DateTime DecisionDate { get; set; }

        public string Status { get; set; }

        public string Disposition {  get; set; }
    }
}
