namespace Backend.Dto.UpdateDtos
{
    public class MemberUpdateDto
    {
        public string MemberName { get; set; }
        public string MemberSurname { get; set; }
        public string MemberEmail { get; set; }
        public string MemberPhone { get; set; }
        public string MemberPassword { get; set; }
        public Guid MembershipID { get; set; }
    }
}
