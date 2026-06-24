using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DischargeDisposition_Backend.Enums;
namespace DischargeDisposition_Backend.Hospital.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }  

        [Required]
        [MaxLength(20)]
        public string Mrn { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateOnly Dob { get; set; }

        [Required]
        public DateTime AdmissionDate { get; set; }

        [Required]
        public DateOnly ExpectedDischargeDate { get; set; }

        [Required]
        public DateTime? ActualDischargeDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [MaxLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Required]
        public byte IsActive { get; set; } = 1;

        [Required]
        public byte DeptId { get; set; }

        [ForeignKey(nameof(DeptId))]
        public Department Department { get; set; } = null!;

        public ICollection<DispositionDecision> DispositionDecisions { get; set; } = new List<DispositionDecision>();

        public ICollection<Referral> Referrals { get; set; } = new List<Referral>();

        public ICollection<AuthorizationTracking> AuthorizationTrackings { get; set; } = new List<AuthorizationTracking>();

        public ICollection<PatientDelay> PatientDelays { get; set; }  = new List<PatientDelay>();
        public LengthOfStayTracking? lengthOfStayTracking { get; set; }

    }
}