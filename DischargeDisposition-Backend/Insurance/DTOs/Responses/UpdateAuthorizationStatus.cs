using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Insurance.DTOs.Responses
{
    public class UpdateAuthorizationStatus
    {
        public AuthorizationStatus Status { get; set; }

        public string? ReasonCode { get; set; }

        public string? Notes { get; set; }
    }
}
