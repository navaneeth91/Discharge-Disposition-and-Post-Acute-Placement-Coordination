namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class PostAcuteProviderResponse
    {
        public int ProviderId { get; set; }

        public string ProviderName { get; set; }
            = string.Empty;

        public bool IsActive { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? ContactPerson { get; set; }

        public string? AddressLine { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public int DispositionTypeId { get; set; }

        public string DispositionName { get; set; }
            = string.Empty;
    }
}