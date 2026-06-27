using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class AuthorizationTrackingResponseDto
    {
        public long AuthorizationId { get; set; }

        public int ReferralId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string MRN { get; set; } = string.Empty;

        public string PayerName { get; set; } = string.Empty;

        public string ExternalAuthorizationId { get; set; } = string.Empty;

        public AuthorizationStatus Status { get; set; }

        public DateTime RequestedDate { get; set; }

        public DateTime? ResponseDate { get; set; }

        public string? DenialReason { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
