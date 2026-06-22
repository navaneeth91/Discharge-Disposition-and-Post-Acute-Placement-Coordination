using DischargeDisposition_Backend.Enums;
namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class AuthorizationWebhook
    {
        public int AuthorizationRequestId { get; set; }

        public AuthorizationStatus Status { get; set; }

        public DateTime DecisionDate { get; set; }

        public string? ReasonCode { get; set; }

        public string? Notes { get; set; }
    }
}