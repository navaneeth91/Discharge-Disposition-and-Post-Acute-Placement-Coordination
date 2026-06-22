using System.ComponentModel.DataAnnotations;
using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Hospital.DTOs.Requests
{
    public class UpdateDispositionDecisionRequest
    {
        [Required]
        public int DispositionTypeId { get; set; }

        [Required]
        public AuthorizationStatus Status { get; set; }

        [Required]
        public DateOnly ExpectedTransitionDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        public byte DepartmentId { get; set; }
    }
}