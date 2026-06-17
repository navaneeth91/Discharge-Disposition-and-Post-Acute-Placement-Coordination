using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace DischargeDisposition_Backend.Insurance.Models
{
    public class MemberCoverage
    {
        [Key]
        public int CoverageId { get; set; }
        public int MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        public virtual Member member { get; set; } = null!;
        public int PlanId { get; set; }
        [ForeignKey(nameof(PlanId))]
        public virtual Plan plan { get; set; } = null!;
    }
}
