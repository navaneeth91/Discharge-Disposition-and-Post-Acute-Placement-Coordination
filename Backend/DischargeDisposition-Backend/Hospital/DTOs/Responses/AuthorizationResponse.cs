using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class AuthorizationResponse
    {
        public long AuthorizationId { get; set; }

        public string ExternalAuthorizationId { get; set; }
            = string.Empty;

        public AuthorizationStatus Status { get; set; }

        public DateTime RequestedDate { get; set; }

        public DateTime? ResponseDate { get; set; }

        public string PayerName { get; set; }
            = string.Empty;

        public string PatientName { get; set; }
            = string.Empty;

        public string? DenialReason { get; set; }
    }
}
