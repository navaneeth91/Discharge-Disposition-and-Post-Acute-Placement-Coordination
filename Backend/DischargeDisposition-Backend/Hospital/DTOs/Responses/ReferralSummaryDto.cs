namespace DischargeDisposition_Backend.Hospital.Dtos.Referral
{
    public class ReferralSummaryDto
    {
        public int ReferralId { get; set; }
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}