using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace DischargeDisposition_Backend.Insurance.Models
{
    public class MemberCoverage
    {
        public int CoverageId { get; set; }
        public int MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        public virtual Member Member { get; set; }
        public int PlanId { get; set; }
        [ForeignKey(nameof(PlanId))]
        public virtual Plane Plan { get; set; }
    }
}
