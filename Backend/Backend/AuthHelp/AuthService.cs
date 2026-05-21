using Backend.Data.Context;
using Backend.Dto.BasicDtos;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Backend.AuthHelp
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly MyDbContext _context;

        public AuthService(IConfiguration configuration, MyDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<TokenModelDto?> AuthenticateAsync(LoginDto loginDto)
        {
            var admin = await _context.Admins.SingleOrDefaultAsync(a => a.AdminEmail == loginDto.Email);
            if (admin != null && PasswordHasher.VerifyPassword(loginDto.Password, admin.AdminHashedPassword))
            {
                var accessToken = GenerateToken(admin.AdminEmail, "Admin", admin.AdminId);
                var refreshToken = GenerateRefreshToken();

                admin.RefreshToken = refreshToken;
                admin.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _context.SaveChangesAsync();

                return new TokenModelDto { AccessToken = accessToken, RefreshToken = refreshToken, Role = "Admin" };
            }

            var member = await _context.Members.SingleOrDefaultAsync(m => m.MemberEmail == loginDto.Email);
            if (member != null && PasswordHasher.VerifyPassword(loginDto.Password, member.MemberHashedPassword))
            {
                var accessToken = GenerateToken(member.MemberEmail, "Member", member.MemberId);
                var refreshToken = GenerateRefreshToken();

                member.RefreshToken = refreshToken;
                member.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _context.SaveChangesAsync();

                return new TokenModelDto { AccessToken = accessToken, RefreshToken = refreshToken, Role = "Member" };
            }

            return null; 
        }

        public async Task<TokenModelDto?> RefreshTokensAsync(TokenModelDto tokenModel)
        {
            ClaimsPrincipal principal;
            try
            {
                principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            }
            catch (Exception)
            {
                return null;
            }

            var email = principal.FindFirstValue(ClaimTypes.Email);
            var role = principal.FindFirstValue(ClaimTypes.Role);
            var idString = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role) || !Guid.TryParse(idString, out var id))
            {
                return null;
            }

            if (role == "Admin")
            {
                var admin = await _context.Admins.FindAsync(id);
                if (admin == null || admin.RefreshToken != tokenModel.RefreshToken || admin.RefreshTokenExpiryTime <= DateTime.UtcNow)
                    return null;

                var newAccessToken = GenerateToken(email, role, id);
                var newRefreshToken = GenerateRefreshToken();

                admin.RefreshToken = newRefreshToken;
                await _context.SaveChangesAsync();

                return new TokenModelDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken, Role = role };
            }
            else if (role == "Member")
            {
                var member = await _context.Members.FindAsync(id);
                if (member == null || member.RefreshToken != tokenModel.RefreshToken || member.RefreshTokenExpiryTime <= DateTime.UtcNow)
                    return null;

                var newAccessToken = GenerateToken(email, role, id);
                var newRefreshToken = GenerateRefreshToken();

                member.RefreshToken = newRefreshToken;
                await _context.SaveChangesAsync();

                return new TokenModelDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken, Role = role };
            }

            return null;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false,

                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private string GenerateToken(string email, string role, Guid id)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}