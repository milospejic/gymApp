using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.BasicDtos
{
    public class MembershipPlanDto
    {
        public Guid PlanID { get; set; }
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public double PlanPrice { get; set; }
        public bool ForDeletion { get; set; }
        public AdminDto Admin { get; set; }

    }
}
