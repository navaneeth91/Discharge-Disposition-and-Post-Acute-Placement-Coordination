using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.Models;
using System.ComponentModel.DataAnnotations;

namespace DischargeDisposition_Backend.Insurance.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = null!;
        [Required]
        [StringLength(30)]
        public string PolicyNumber { get; set; } = null!;
        [Required]
        public DateOnly DOB { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = null!;
        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = null!;

        public virtual ICollection<MemberCoverage> MemberCoverages { get; set; }
        = new List<MemberCoverage>();

    }
}
