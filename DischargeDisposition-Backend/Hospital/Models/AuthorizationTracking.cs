using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Hospital.Models
{
    public class AuthorizationTracking
    {
        [Key]
        public long AuthorizationId { get; set; }
        public int PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; } = null!;
        public int ReferralId { get; set; }
        [ForeignKey(nameof(ReferralId))]
        public virtual Referral Referral { get; set; } = null!;
        public int PayerId { get; set; }
        [ForeignKey(nameof(PayerId))]
        public virtual Payer Payer { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string ExternalAuthorizationId { get; set; } = null!;
       
        //public int DispositionTypeId { get; set; }
        //[ForeignKey(nameof(DispositionTypeId))]
        //public virtual DispositionType DispositionType { get; set; } = null!;
        public AuthorizationStatus Status { get; set; } 
        public DateTime RequestedDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        [StringLength(500)]
        public string? DenialReason {  get; set; }
        public DateTime LastUpdated { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
    }
}
