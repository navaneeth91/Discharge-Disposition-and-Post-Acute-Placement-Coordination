using System.ComponentModel.DataAnnotations;
namespace DischargeDisposition_Backend.Hospital.Models
{
    public class DispositionType
    {
        [Key]
        public int DispositionTypeId { get; set; }
        [Required]
        [StringLength(100)]
        public String DispositionName { get; set; }

        public virtual ICollection<PostAcuteProvider> PostAcuteProviders { get; set; }
        = new List<PostAcuteProvider>();

        
        public virtual ICollection<DispositionDecision> DispositionDecisions { get; set; }
            = new List<DispositionDecision>();

        public virtual ICollection<AuthorizationTracking> AuthorizationTrackings { get; set; }
        = new List<AuthorizationTracking>();
    }
}
