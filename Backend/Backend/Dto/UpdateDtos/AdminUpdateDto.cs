namespace Backend.Dto.UpdateDtos
{
    using System;
    using System.ComponentModel.DataAnnotations;

    namespace Backend.Dto.UpdateDtos
    {
        public class AdminUpdateDto
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

        }
    }

}
