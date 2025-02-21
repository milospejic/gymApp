namespace Backend.Dto.CreateDtos
{
    public class MembershipCreateDto
    {
        public DateTime MembershipFrom { get; set; }
        public DateTime MembershipTo { get; set; }
        public string MembershipStatus { get; set; }
        public Guid MembershipPlanID { get; set; }
    }
}
