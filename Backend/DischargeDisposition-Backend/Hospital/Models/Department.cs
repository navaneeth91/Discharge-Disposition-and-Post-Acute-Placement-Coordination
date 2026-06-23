using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DischargeDisposition_Backend.Hospital.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte DeptId { get; set; } // TINYINT IDENTITY(1,1)
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }   
}