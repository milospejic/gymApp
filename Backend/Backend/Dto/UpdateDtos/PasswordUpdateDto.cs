using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.UpdateDtos
{
    /// <summary>
    /// DTO for updating a user's password.
    /// </summary>
    public class PasswordUpdateDto
    {
        /// <summary>
        /// The user's password.
        /// </summary>
        [Required(ErrorMessage = "Current password is required.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// The new password for the user.
        /// </summary>
        [Required(ErrorMessage = "New password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        /// <summary>
        /// The new password for the user again.
        /// </summary>
        [Required(ErrorMessage = "Password confirmation is required.")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
