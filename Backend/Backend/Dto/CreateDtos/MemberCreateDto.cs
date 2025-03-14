using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.CreateDtos
{
    /// <summary>
    /// Data Transfer Object for creating a Member entity.
    /// </summary>
    public class MemberCreateDto
    {
        /// <summary>
        /// First name of the member.
        /// </summary>
        [Required(ErrorMessage = "Member name is required.")]
        [StringLength(100, ErrorMessage = "Member name can't be longer than 100 characters.")]
        public string MemberName { get; set; }

        /// <summary>
        /// Surname of the member.
        /// </summary>
        [Required(ErrorMessage = "Member surname is required.")]
        [StringLength(100, ErrorMessage = "Member surname can't be longer than 100 characters.")]
        public string MemberSurname { get; set; }

        /// <summary>
        /// Email address of the member.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string MemberEmail { get; set; }

        /// <summary>
        /// Phone number of the member.
        /// </summary>
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string MemberPhone { get; set; }

        /// <summary>
        /// Secure password for the member.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string MemberHashedPassword { get; set; }

        /// <summary>
        /// Membership details for the member.
        /// </summary>
        [Required(ErrorMessage = "Picking membership plan and duration is required.")]
        public MembershipCreateDto Membership { get; set; }

    }
}
