namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class PatientByDeptIdResponse
    {
        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;
        public DateOnly Dob { get; set; }

        public byte Status { get; set; }
        public byte DeptId { get; set; }
        public bool HasDecision { get; set; }

        public int? DecisionId { get; set; }
    }
}
