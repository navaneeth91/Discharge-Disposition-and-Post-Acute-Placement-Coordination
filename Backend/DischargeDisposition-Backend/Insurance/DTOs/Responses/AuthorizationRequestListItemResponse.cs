using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Insurance.DTOs.Responses
{
    public class AuthorizationRequestListItemResponse
    {
        public int AuthorizationRequestId { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        public string PolicyNumber { get; set; } = string.Empty;

        public string RequestingOrganization { get; set; } = string.Empty;

        public string ServiceType { get; set; } = string.Empty;

        public DateTime RequestedDate { get; set; }

        public AuthorizationStatus Status { get; set; }

        public DateTime? LatestDecisionDate { get; set; }

        public string? ReasonCode { get; set; }

        public string? Notes { get; set; }
    }
}