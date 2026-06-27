namespace DischargeDisposition_Backend.Insurance.DTOs.Responses
{

    public class PlanResponse
    {
        public int PlanId { get; set; }

        public int InsuranceProviderId { get; set; }

        public string PlanName { get; set; } = string.Empty;

        public string PlanType { get; set; } = string.Empty;

        public string ProviderName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}