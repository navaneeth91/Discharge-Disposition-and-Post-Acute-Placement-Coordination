namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class ReferralDetailsDto
    {
        // Referral
        public int ReferralId { get; set; }
        public DateTime ReferralDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;

        // Patient
        public int PatientId { get; set; }
        public string Mrn { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public DateOnly Dob { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        // Admission
       
        public DateOnly ExpectedDischargeDate { get; set; }
        
        public string Department { get; set; } = string.Empty;

        // Provider
        public string ProviderName { get; set; } = string.Empty;
        public string CareManager { get; set; } = string.Empty;
    }
}