using System.ComponentModel.DataAnnotations;
using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Hospital.DTOs.Requests
{
    public class CreateReferralDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        public int CareManagerId { get; set; }

        // Optional: allow caller to provide created date; defaulting to UtcNow in service if not supplied.
        public DateTime? CreatedDate { get; set; }

        [Required]
        public AuthorizationStatus Status { get; set; }

        [Required]
        public Priority Priority { get; set; }
    }
}