using Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Membership
    {
        [Key]
        public Guid MembershipID { get; set; }
        public DateTime MembershipFrom { get; set; }
        public DateTime MembershipTo { get; set;}
        public string MembershipStatus { get; set; }
        public Duration PlanDuration { get; set; }
        public double MembershipFee { get; set; }
        public Boolean IsFeePaid { get; set; }
        public Guid MembershipPlanID { get; set; }
    }
}
