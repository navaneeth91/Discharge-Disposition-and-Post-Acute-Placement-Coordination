namespace DischargeDisposition_Backend.Insurance.DTOs.Requests
{



    public class MemberCoverage
    {
        public int CoverageId { get; set; }

        public string PlanName { get; set; } = string.Empty;

        public string PlanType { get; set; } = string.Empty;

        public string InsuranceProvider { get; set; } = string.Empty;
    }
}