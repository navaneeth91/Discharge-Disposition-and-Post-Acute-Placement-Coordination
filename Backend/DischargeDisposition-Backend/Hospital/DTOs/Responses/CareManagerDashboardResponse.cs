namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class CareManagerDashboardResponse
    {
        public int AssignedPatients { get; set; }

        public int ReadyForReferral { get; set; }

        public int PendingReferrals { get; set; }

        public int ActiveDelays { get; set; }
    }
}