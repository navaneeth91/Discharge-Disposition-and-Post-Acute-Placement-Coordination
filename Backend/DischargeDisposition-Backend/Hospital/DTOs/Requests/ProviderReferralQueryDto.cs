namespace DischargeDisposition_Backend.Hospital.DTOs.Requests
{
    public class ProviderReferralQueryDto
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public string? Status { get; set; }
    }
}