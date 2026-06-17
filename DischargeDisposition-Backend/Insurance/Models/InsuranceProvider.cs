
using System.ComponentModel.DataAnnotations;

namespace DischargeDisposition_Backend.Insurance.Models
{
    public class InsuranceProvider
    {
        [Key]
        public int InsuranceProviderId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProviderName { get; set; }

        [Required]
        [StringLength(20)]
        public string ProviderCode { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
            = new List<Plan>();
    }
}
