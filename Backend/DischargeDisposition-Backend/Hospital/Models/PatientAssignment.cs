using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DischargeDisposition_Backend.Hospital.Models
{
    public class PatientAssignment
    {
        [Key]
        public long AssignmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int CareManagerId { get; set; }

        [Required]
        public int AssignedBy { get; set; }

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UnassignedDate { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }

        [ForeignKey(nameof(CareManagerId))]
        public virtual User CareManager { get; set; }

        [ForeignKey(nameof(AssignedBy))]
        public virtual User AssignedByUser { get; set; }
    }
}