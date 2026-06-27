using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Insurance.DTOs.Responses
{
    public class MemberSearchResponse
    {
        public int MemberId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string PolicyNumber { get; set; } = string.Empty;

        public Gender Gender { get; set; }

        public DateOnly DOB { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int CoverageCount { get; set; }

        public int AuthorizationCount { get; set; }
    }
}