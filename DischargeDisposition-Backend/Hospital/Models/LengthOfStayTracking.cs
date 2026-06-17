using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DischargeDisposition_Backend.Hospital.Models
{
    public class LengthOfStayTracking
    {
        [Key]
        public long TrackingId { get; set; }

        [Required]
        public long PatientId { get; set; }

        [Required]
        public short VarianceDays { get; set; }

        [Required]
        public DateTime LastCalculatedDate { get; set; }

        // Navigation Property
        public Patient Patient { get; set; } = null!;


    }
}