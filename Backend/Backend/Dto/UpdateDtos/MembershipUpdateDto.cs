namespace Backend.Dto.UpdateDtos
{
    public class MembershipUpdateDto
    {
        public DateTime MembershipFrom { get; set; }
        public DateTime MembershipTo { get; set; }
        public string MembershipStatus { get; set; }
        public Guid MembershipPlanID { get; set; }
    }
}
