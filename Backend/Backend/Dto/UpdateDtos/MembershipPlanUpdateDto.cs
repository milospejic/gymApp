namespace Backend.Dto.UpdateDtos
{
    public class MembershipPlanUpdateDto
    {
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public string PlanDuration { get; set; }
        public double PlanPrice { get; set; }
        public Guid AdminID { get; set; }
    }
}
