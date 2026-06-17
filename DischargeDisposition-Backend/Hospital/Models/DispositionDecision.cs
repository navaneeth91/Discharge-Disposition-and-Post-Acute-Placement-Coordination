using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Hospital.Models
{
    public class DispositionDecision
    {
        [Key]
        public int DecisionId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DispositionTypeId { get; set; }

        [Required]
        public int ClinicianId { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public DateTime DecisionDate { get; set; }

        [Required]
        public AuthorizationStatus Status { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        public DateOnly ExpectedTransitionDate { get; set; }

        // Navigation Properties

        [ForeignKey(nameof(PatientId))]
        public virtual Patient patient { get; set; } = null!;

        [ForeignKey(nameof(DispositionTypeId))]
        public virtual DispositionType dispositionType { get; set; } = null!;

        [ForeignKey(nameof(ClinicianId))]
        public virtual User clinician { get; set; } = null!;

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department department { get; set; } = null!;
    }
}
