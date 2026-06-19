namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class PatientResponseDto
    {
        public int PatientId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public  DateTime AdmissionDate { get; set; }

        public byte IsActive { get; set; }
    }
}
