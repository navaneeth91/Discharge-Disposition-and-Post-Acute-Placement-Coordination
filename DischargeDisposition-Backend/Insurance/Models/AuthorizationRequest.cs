using DischargeDisposition_Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DischargeDisposition_Backend.Insurance.Models
{
    public class AuthorizationRequest
    {
        [Key]
        public int AuthorizationRequestId { get; set; }
        
        public int MemberId { get; set; }

        [Required]
        public  string RequestingOrganization {  get; set; } = string.Empty;

        [Required]
        public string ServiceType { get; set; } = string.Empty;

        [Required]
        public DateTime RequestedDate { get; set; }

        [Required]
        public AuthorizationStatus Status {  get; set; } 

        [ForeignKey(nameof(MemberId))]
        public Member member { get; set; } = null!;

        public ICollection<AuthorizationDecision> AuthorizationDecisions { get; set; } = new List<AuthorizationDecision>();
    }
}
