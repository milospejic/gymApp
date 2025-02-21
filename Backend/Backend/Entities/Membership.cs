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
        public Guid MembershipPlanID { get; set; }
    }
}
