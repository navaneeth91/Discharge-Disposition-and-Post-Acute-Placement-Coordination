using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DischargeDisposition_Backend.Hospital.Models
{
    public class DelayReasonCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; } // TINYINT IDENTITY(1,1)

        [Required]
        [MaxLength(50)]
        public string ReasonName { get; set; } = string.Empty;
    }
}