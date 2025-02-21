namespace Backend.Dto.BasicDtos
{
    public class MembershipPlanDto
    {
        public Guid PlanID { get; set; }
        public string PlanName { get; set; }

        public string PlanDescription { get; set; }

        public string PlanDuration { get; set; }

        public double PlanPrice { get; set; }
        public Guid AdminID { get; set; }
    }
}
