using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual Patient Patient { get; set; }

        [ForeignKey(nameof(DispositionTypeId))]
        public virtual DispositionType DispositionType { get; set; }

        [ForeignKey(nameof(ClinicianId))]
        public virtual User Clinician { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
    }
}
