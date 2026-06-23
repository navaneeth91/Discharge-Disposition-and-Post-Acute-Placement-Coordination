namespace DischargeDisposition_Backend.Hospital.DTOs.Requests
{
    public class CreateAuthorizationRequest
    {
        public int PatientId { get; set; }

        public int ReferralId { get; set; }

        public int PayerId { get; set; }

        public int MemberId { get; set; }

        public string ServiceType { get; set; } = string.Empty;

        public string RequestingOrganization { get; set; } = string.Empty;
    }
}
