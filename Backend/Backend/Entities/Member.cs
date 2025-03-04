using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities
{
    [Index(nameof(MemberEmail), IsUnique = true, Name = "IX_Member_Email_Unique")]
    public class Member
    {
        [Key]
        public Guid MemberId { get; set; }

        [Required(ErrorMessage = "Member name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string MemberName { get; set; }

        [Required(ErrorMessage = "Member surname is required")]
        [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters")]
        public string MemberSurname { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string MemberEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string MemberPhone { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string MemberHashedPassword { get; set; }

        [Required(ErrorMessage = "Membership ID is required")]
        public Guid MembershipID { get; set; }

        [ForeignKey("MembershipID")]
        public virtual Membership Membership { get; set; }
    }
}
