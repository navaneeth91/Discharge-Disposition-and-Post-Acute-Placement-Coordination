using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DischargeDisposition_Backend.Enums;
namespace DischargeDisposition_Backend.Hospital.Models
{

    public class Referral
    {
        [Key]
        public int ReferralId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        public int CareManagerId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public AuthorizationStatus Status { get; set; }

        [Required]
        [MaxLength(10)]
        public Priority Priority { get; set; }

        // Navigation Properties

        [ForeignKey(nameof(PatientId))]
        public Patient patient { get; set; } = null!;

        [ForeignKey(nameof(ProviderId))]
        public PostAcuteProvider provider { get; set; } = null!;

        [ForeignKey(nameof(CareManagerId))]
        public User careManager { get; set; } = null!;
        public virtual AuthorizationTracking? authorizationTrackings { get; set; }
    }
}