using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class ReferralTrackingResponseDto
    {
        public int ReferralId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public int ProviderId { get; set; }

        public string ProviderName { get; set; } = string.Empty;

        public string Status { get; set; }

        public string Priority { get; set; }

        public DateTime CreatedDate { get; set; }

        // New property only for Referral Tracking
        public bool AuthorizationCreated { get; set; }
    }
}