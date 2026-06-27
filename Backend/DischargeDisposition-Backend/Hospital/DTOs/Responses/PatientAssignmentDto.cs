namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class PatientAssignmentDto
    {
        public long AssignmentId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public int CareManagerId { get; set; }

        public string CareManagerName { get; set; } = string.Empty;

        public DateTime AssignedDate { get; set; }

        public bool IsActive { get; set; }
    }
}