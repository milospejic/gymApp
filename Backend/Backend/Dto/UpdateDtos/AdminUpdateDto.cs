using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.UpdateDtos
{
    /// <summary>
    /// Data Transfer Object for updaing an Admin entity.
    /// </summary>
    public class AdminUpdateDto
    {
        /// <summary>
        /// First name of the admin.
        /// </summary>
        [Required(ErrorMessage = "Admin name is required.")]
        [StringLength(100, ErrorMessage = "Admin name can't be longer than 100 characters.")]
        public string AdminName { get; set; }

        /// <summary>
        /// Surname of the admin.
        /// </summary>
        [Required(ErrorMessage = "Admin surname is required.")]
        [StringLength(100, ErrorMessage = "Admin surname can't be longer than 100 characters.")]
        public string AdminSurname { get; set; }

        /// <summary>
        /// Email address of the admin.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string AdminEmail { get; set; }

        /// <summary>
        /// Phone number of the admin.
        /// </summary>
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string AdminPhone { get; set; }

    }
}


