namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class HospitalDashboard
    {
        public int TotalPatients { get; set; }

        public int PendingReferrals { get; set; }

        public int PendingAuthorizations { get; set; }

        public int ActiveDelays { get; set; }
    }
}