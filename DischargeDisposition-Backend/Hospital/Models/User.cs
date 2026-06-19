using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DischargeDisposition_Backend.Hospital.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public byte DeptId { get; set; }

        [ForeignKey(nameof(DeptId))]
        public Department department { get; set; } = null!;

        [Required]
        public byte RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role role { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(60)]
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<PatientDelay> PatientDelays { get; set; } = new List<PatientDelay>();

        public ICollection<DispositionDecision> DispositionDecisions { get; set; } = new List<DispositionDecision>();

        public ICollection<Referral> Referrals { get; set; } = new List<Referral>();

        // Inverse navigation for PostAcuteProvider.UserId
        public ICollection<PostAcuteProvider> PostAcuteProviders { get; set; } = new List<PostAcuteProvider>();
    }
}
