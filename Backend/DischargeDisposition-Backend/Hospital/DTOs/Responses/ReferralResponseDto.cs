using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class ReferralResponseDto
    {
        public int ReferralId { get; set; }
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public int CareManagerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public AuthorizationStatus Status { get; set; }
        public Priority Priority { get; set; }

        // Simple relation summaries
        public string? PatientName { get; set; }
        public string? ProviderName { get; set; }
        public string? CareManagerName { get; set; }
    }
}