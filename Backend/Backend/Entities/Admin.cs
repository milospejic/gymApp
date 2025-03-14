using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities
{
    /// <summary>
    /// Represents an administrator with specific privileges and authentication details.
    /// </summary>
    [Index(nameof(AdminEmail), IsUnique = true, Name = "IX_Admin_Email_Unique")]
    public class Admin
    {
        /// <summary>
        /// Unique identifier for the admin.
        /// </summary>
        [Key]
        public Guid AdminId { get; set; }

        /// <summary>
        /// First name of the admin.
        /// </summary>
        [Required(ErrorMessage = "Admin name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string AdminName { get; set; }

        /// <summary>
        /// Surname of the admin.
        /// </summary>
        [Required(ErrorMessage = "Admin surname is required")]
        [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters")]
        public string AdminSurname { get; set; }

        /// <summary>
        /// Email address of the admin.
        /// </summary>
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string AdminEmail { get; set; }

        /// <summary>
        /// Phone number of the admin.
        /// </summary>
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string AdminPhone { get; set; }

        /// <summary>
        /// Hashed password of the admin.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string AdminHashedPassword { get; set; }
    }
}