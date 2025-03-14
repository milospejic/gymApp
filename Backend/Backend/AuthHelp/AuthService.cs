using Backend.Data.Context;
using Backend.Data.IRepository;
using Backend.Data.Repository;
using Backend.Dto.BasicDtos;
using Backend.Entities;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.AuthHelp
{
    /// <summary>
    /// Implements the <see cref="IAuthService"/> interface for managing auth-related operations.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly MyDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration settings.</param>
        /// <param name="context">Database context for accessing user data.</param>
        public AuthService(IConfiguration configuration, MyDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }

        /// <summary>
        /// Validates a member's login credentials.
        /// </summary>
        /// <param name="loginDto">The login DTO containing email and password.</param>
        /// <returns>The authenticated <see cref="Member"/> entity if credentials are valid; otherwise, null.</returns>
        public async Task<Member> IsMember(LoginDto loginDto)
        {
            var member = await context.Members.SingleOrDefaultAsync(m => m.MemberEmail == loginDto.Email);
            if (member != null && PasswordHasher.VerifyPassword(loginDto.Password, member.MemberHashedPassword))
                return member;
            return null;
        }

        /// <summary>
        /// Validates an admin's login credentials.
        /// </summary>
        /// <param name="loginDto">The login DTO containing email and password.</param>
        /// <returns>The authenticated <see cref="Admin"/> entity if credentials are valid; otherwise, null.</returns>
        public async Task<Admin> IsAdmin(LoginDto loginDto)
        {
            var admin = await context.Admins.SingleOrDefaultAsync(a => a.AdminEmail == loginDto.Email);
            if (admin != null && PasswordHasher.VerifyPassword(loginDto.Password, admin.AdminHashedPassword))
                return admin;
            return null;
        }

        /// <summary>
        /// Generates a JWT token for an authenticated user.
        /// </summary>
        /// <param name="email">The email of the authenticated user.</param>
        /// <param name="role">The role of the authenticated user.</param>
        /// <param name="id">The unique identifier of the authenticated user.</param>
        /// <returns>A JWT token as a string.</returns>
        public string GenerateToken(string email, string role, Guid id)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
