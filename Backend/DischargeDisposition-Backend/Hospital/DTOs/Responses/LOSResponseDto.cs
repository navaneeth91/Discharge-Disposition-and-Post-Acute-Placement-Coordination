namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class LOSResponseDto
    {
        public int PatientId { get; set; }

        public string PatientName { get; set; }
        

        public DateTime AdmissionDate { get; set; }

        public short VarianceDays { get; set; }

        public DateTime LastCalculatedDate { get; set; }
    }
}
