using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DischargeDisposition_Backend.Hospital.Models;

public class PatientDelay
{
    [Key]
    public int PatientDelayId { get; set; }

    [Required]
    public int PatientId { get; set; }


    [Required]
    public byte DelayReasonId { get; set; }

    [Required]
    public int ReportedBy { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    // Navigation Properties

    [ForeignKey(nameof(PatientId))]
    public Patient patient { get; set; } = null!;

    [ForeignKey(nameof(DelayReasonId))]
    public DelayReasonCode delayReason { get; set; } = null!;

    [ForeignKey(nameof(ReportedBy))]
    public User reportedUser { get; set; } = null!;
}