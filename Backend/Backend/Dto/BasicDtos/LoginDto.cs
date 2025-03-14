namespace Backend.Dto.BasicDtos
{
    /// <summary>
    /// Data Transfer Object for login credentials.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password for authentication.
        /// </summary>
        public string Password { get; set; }
    }
}