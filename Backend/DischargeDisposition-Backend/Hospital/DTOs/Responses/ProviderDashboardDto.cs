namespace DischargeDisposition_Backend.Hospital.DTOs.Responses;

public class ProviderDashboardDto
{
    public int TotalReferrals { get; set; }

    public int PendingReferrals { get; set; }

    public int ApprovedReferrals { get; set; }

    public int RejectedReferrals { get; set; }
}