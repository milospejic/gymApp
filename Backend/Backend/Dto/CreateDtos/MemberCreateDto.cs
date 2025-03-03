using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.CreateDtos
{
    public class MemberCreateDto
    {
        [Required(ErrorMessage = "Member name is required.")]
        [StringLength(100, ErrorMessage = "Member name can't be longer than 100 characters.")]
        public string MemberName { get; set; }

        [Required(ErrorMessage = "Member surname is required.")]
        [StringLength(100, ErrorMessage = "Member surname can't be longer than 100 characters.")]
        public string MemberSurname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string MemberEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string MemberPhone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string MemberHashedPassword { get; set; }

        [Required(ErrorMessage = "Picking membership plan and duration is required.")]
        public MembershipCreateDto Membership { get; set; }

    }
}
