using System.ComponentModel.DataAnnotations;

namespace DischargeDisposition_Backend.DTOs.Requests
{
    public class CreateDispositionDecisionRequest
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DispositionTypeId { get; set; }

        [Required]
        public int ClinicianId { get; set; }

        [Required]
        public byte DepartmentId { get; set; }

        [Required]
        public DateOnly ExpectedTransitionDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}