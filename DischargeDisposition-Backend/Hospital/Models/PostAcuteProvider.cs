using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DischargeDisposition_Backend.Models
{
    public class PostAcuteProvider
    {
        [Key]
        public int ProviderId { get; set; }

        [Required]
        public int DispositionTypeId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProviderName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(150)]
        public string? ContactPerson { get; set; }

        [StringLength(200)]
        public string? AddressLine { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

       
        [ForeignKey(nameof(DispositionTypeId))]
        public virtual DispositionType DispositionType { get; set; }

    }
}
