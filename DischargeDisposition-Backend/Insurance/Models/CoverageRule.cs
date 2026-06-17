using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DischargeDisposition_Backend.Insurance.Models;

public class CoverageRule
{
    [Key]
    public int RuleId { get; set; }

    [Required]
    public int PlanId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ServiceType { get; set; } = string.Empty;

    [Required]
    public bool RequiresAuthorization { get; set; }

    // Navigation Property
    [ForeignKey(nameof(PlanId))]
    public Plan plan { get; set; } = null!;
}