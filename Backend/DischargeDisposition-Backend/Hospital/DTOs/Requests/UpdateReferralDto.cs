using System.ComponentModel.DataAnnotations;
using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Hospital.DTOs.Requests
{
    public class UpdateReferralDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        public int CareManagerId { get; set; }

        public DateTime? CreatedDate { get; set; }

        [Required]
        public AuthorizationStatus Status { get; set; }

        [Required]
        public Priority Priority { get; set; }
    }
}