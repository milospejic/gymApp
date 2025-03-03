using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Member
    {
        [Key]
        public Guid MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberSurname {  get; set; }
        public string MemberEmail { get; set; }
        public string MemberPhone { get; set; }
        public string MemberHashedPassword { get; set; }
        public Guid MembershipID { get; set; }
    }
}
