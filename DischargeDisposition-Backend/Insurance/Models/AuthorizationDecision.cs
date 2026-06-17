using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DischargeDisposition_Backend.Enums;
namespace DischargeDisposition_Backend.Insurance.Models;

public class AuthorizationDecision
{
    [Key]
    public int DecisionId { get; set; }

    [Required]
    public int AuthorizationRequestId { get; set; }

    [Required]
    public AuthorizationStatus DecisionStatus { get; set; }

    [Required]
    public DateTime DecisionDate { get; set; }

    [MaxLength(50)]
    public string? ReasonCode { get; set; }

    [MaxLength(100)]
    public string? Notes { get; set; }

    // Navigation Property

    [ForeignKey(nameof(AuthorizationRequestId))]
    public AuthorizationRequest authorizationRequest { get; set; } = null!;
}