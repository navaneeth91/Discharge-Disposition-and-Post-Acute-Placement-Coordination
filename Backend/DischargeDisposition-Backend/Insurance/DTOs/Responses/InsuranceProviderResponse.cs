namespace DischargeDisposition_Backend.Insurance.DTOs.Responses
{

    public class InsuranceProviderResponse
    {
        public int InsuranceProviderId { get; set; }

        public string ProviderName { get; set; } = string.Empty;

        public string ProviderCode { get; set; } = string.Empty;
    }
}
