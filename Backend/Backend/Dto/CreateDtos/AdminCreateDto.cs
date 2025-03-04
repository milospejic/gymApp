using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.CreateDtos
{
    public class AdminCreateDto
    {
        [Required(ErrorMessage = "Admin name is required.")]
        [StringLength(100, ErrorMessage = "Admin name can't be longer than 100 characters.")]
        public string AdminName { get; set; }

        [Required(ErrorMessage = "Admin surname is required.")]
        [StringLength(100, ErrorMessage = "Admin surname can't be longer than 100 characters.")]
        public string AdminSurname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string AdminEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string AdminPhone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8s characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string AdminHashedPassword { get; set; }
    }
}

