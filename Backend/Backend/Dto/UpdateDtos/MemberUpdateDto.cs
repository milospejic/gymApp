using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.UpdateDtos
{
    /// <summary>
    /// Data Transfer Object for updating a Memberr entity.
    /// </summary>
    public class MemberUpdateDto
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
        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(20, ErrorMessage = "Phone number can't be longer than 20 characters.")]
        public string MemberPhone { get; set; }

    }
}
