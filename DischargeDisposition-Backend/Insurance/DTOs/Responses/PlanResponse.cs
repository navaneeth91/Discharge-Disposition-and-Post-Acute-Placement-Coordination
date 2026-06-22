namespace DischargeDisposition_Backend.Insurance.DTOs
{

    public class PlanResponse
    {
        public int PlanId { get; set; }

        public string PlanName { get; set; } = string.Empty;

        public string PlanType { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}