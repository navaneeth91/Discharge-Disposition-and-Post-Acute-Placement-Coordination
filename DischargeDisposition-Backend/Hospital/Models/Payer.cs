
using System.ComponentModel.DataAnnotations;
namespace DischargeDisposition_Backend.Hospital.Models
{
    public class Payer
    {
        [Key]
        public int PayerId { get; set; }

        [Required]
        [StringLength(150)]
        public string PayerName { get; set; } = null!;

        [Required]
        [Phone]
        [StringLength(15)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public virtual ICollection<AuthorizationTracking> AuthorizationTrackings { get; set; }
            = new List<AuthorizationTracking>();
    }
}