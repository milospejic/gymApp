using Backend.Dto.BasicDtos;
using Backend.Entities;

namespace Backend.AuthHelp
{
    /// <summary>
    /// Defines authentication-related operations, including token generation and user validation.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Generates a JWT token for the authenticated user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="role">The role of the user (e.g., Admin, Member).</param>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>A JWT token as a string.</returns>
        string GenerateToken(string email, string role, Guid id);

        /// <summary>
        /// Validates a member's login credentials.
        /// </summary>
        /// <param name="loginDto">The login data transfer object containing email and password.</param>
        /// <returns>The authenticated Member entity if credentials are valid; otherwise, null.</returns>
        Task<Member> IsMember(LoginDto loginDto);

        /// <summary>
        /// Validates an admin's login credentials.
        /// </summary>
        /// <param name="loginDto">The login data transfer object containing email and password.</param>
        /// <returns>The authenticated Admin entity if credentials are valid; otherwise, null.</returns>
        Task<Admin> IsAdmin(LoginDto loginDto);

    }
}
