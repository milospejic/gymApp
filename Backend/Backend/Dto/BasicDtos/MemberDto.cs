namespace Backend.Dto.BasicDtos
{
    /// <summary>
    /// Data Transfer Object for Member entity.
    /// </summary>
    public class MemberDto
    {
        /// <summary>
        /// Unique identifier for the member.
        /// </summary>
        public Guid MemberId { get; set; }

        /// <summary>
        /// First name of the member.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Surname of the member.
        /// </summary>
        public string MemberSurname { get; set; }

        /// <summary>
        /// Email address of the member.
        /// </summary>
        public string MemberEmail { get; set; }

        /// <summary>
        /// Contact phone number of the member.
        /// </summary>
        public string MemberPhone { get; set; }

        /// <summary>
        /// Unique identifier for the associated membership.
        /// </summary>
        public Guid MembershipId { get; set; }

        /// <summary>
        /// Membership details associated with the member.
        /// </summary>
        public MembershipDto Membership { get; set; }
    }
}
