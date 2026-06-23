using DischargeDisposition_Backend.Insurance.DTOs.Requests;

namespace DischargeDisposition_Backend.Insurance.DTOs.Responses {


    public class MemberDetailsResponse
    {
        public int MemberId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PolicyNumber { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public DateOnly DOB { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public List<MemberCoverage> Coverages { get; set; }
            = new();
    }
}