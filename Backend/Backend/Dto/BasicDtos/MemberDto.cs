namespace Backend.Dto.BasicDtos
{
    public class MemberDto
    {
        public Guid MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberSurname { get; set; }
        public string MemberEmail { get; set; }
        public string MemberPhone { get; set; }
        public Guid MembershipId { get; set; }
    }
}
