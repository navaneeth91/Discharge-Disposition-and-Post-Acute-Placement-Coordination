using System.ComponentModel.DataAnnotations;

namespace DischargeDisposition_Backend.Hospital.DTOs.Requests
{
    public class CreatePatientDelayRequest
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public byte DelayReasonId { get; set; }

    }
}