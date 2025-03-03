using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class MembershipPlan
    {
        [Key]
        public Guid PlanID { get; set; }
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public double PlanPrice { get; set; }
        public Guid AdminID { get; set; }
    }
}
