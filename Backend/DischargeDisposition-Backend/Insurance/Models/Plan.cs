using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DischargeDisposition_Backend.Insurance.Models
{
    public class Plan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanId { get; set; }

        [Required]
        public int InsuranceProviderId { get; set; }
        [ForeignKey(nameof(InsuranceProviderId))]
        public InsuranceProvider insuranceProvider { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string PlanName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string PlanType { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; } = true;
        public ICollection<MemberCoverage> MemberCoverages { get; set; } = new List<MemberCoverage>();
        public ICollection<CoverageRule> CoverageRules { get; set; } = new List<CoverageRule>();
    }
}