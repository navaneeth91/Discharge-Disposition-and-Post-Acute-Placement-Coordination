using System.ComponentModel.DataAnnotations;

namespace DischargeDisposition_Backend.Hospital.DTOs.Requests
{
    /// <summary>
    /// Request DTO for assigning a Care Manager to a patient.
    /// </summary>
    public class AssignCareManagerRequest
    {
        [Required(ErrorMessage = "Patient Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Patient Id.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Care Manager Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Care Manager Id.")]
        public int CareManagerId { get; set; }

        [Required(ErrorMessage = "Assigned By is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Assigned By user Id.")]
        public int AssignedBy { get; set; }

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }
    }
}