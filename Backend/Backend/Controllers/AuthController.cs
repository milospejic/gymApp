using Backend.AuthHelp;
using Backend.Data.Context;
using Backend.Dto.BasicDtos;
using Backend.Utils.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    /// <summary>
    /// Controller responsible for managing authentication-related operations, 
    /// such as logging in a user (both admin and member).
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ILogger<AuthController> logger;
        private readonly MyDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">ervice for handling authentication logic.</param>
        /// <param name="logger">Logger for capturing controller activity.</param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger, MyDbContext context)
        {
            this.authService = authService;
            this.logger = logger;
            this.context = context;
        }

        /// <summary>
        /// Authenticates the user based on the provided login credentials and generates a JWT token for valid users.
        /// </summary>
        /// <param name="loginDto">DTO containing the user's email and password.</param>
        /// <returns>Returns a JWT token if authentication is successful.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If the provided data is invalid (e.g., missing or incorrect fields).
        /// - 401 Unauthorized: If the credentials are incorrecte.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                throw new BadRequestException($"Validation failed for LoginDto: {string.Join(" | ", errors)}");
            }
            logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

            var admin = await authService.IsAdmin(loginDto);
            if (admin != null)
            {
                var token = authService.GenerateToken(admin.AdminEmail, "Admin", admin.AdminId);
                var refreshToken = authService.GenerateRefreshToken();

                admin.RefreshToken = refreshToken;
                admin.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await context.SaveChangesAsync();

                logger.LogInformation("Admin login successful for email: {Email}", admin.AdminEmail);
                return Ok(new TokenModelDto { AccessToken = token, RefreshToken = refreshToken, Role = "Admin" });
            }

            var member = await authService.IsMember(loginDto);
            if (member != null)
            {
                var token = authService.GenerateToken(member.MemberEmail, "Member", member.MemberId);
                var refreshToken = authService.GenerateRefreshToken();

                member.RefreshToken = refreshToken;
                member.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await context.SaveChangesAsync();

                logger.LogInformation("Member login successful for email: {Email}", member.MemberEmail);
                return Ok(new TokenModelDto { AccessToken = token, RefreshToken = refreshToken, Role = "Member" });
            }

            throw new UnauthorizedAccessException("Invalid credentials");
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenModelDto tokenModel)
        {
            if (tokenModel is null) return BadRequest("Invalid client request");

            string accessToken = tokenModel.AccessToken;
            string refreshToken = tokenModel.RefreshToken;

            ClaimsPrincipal principal;
            try 
            {
                principal = authService.GetPrincipalFromExpiredToken(accessToken);
            }
            catch (Exception)
            {
                return BadRequest("Invalid access token format.");
            }

            var email = principal.FindFirstValue(ClaimTypes.Email);
            var role = principal.FindFirstValue(ClaimTypes.Role);
            var id = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));

            if (role == "Admin")
            {
                var admin = await context.Admins.FindAsync(id);
                if (admin == null || admin.RefreshToken != refreshToken || admin.RefreshTokenExpiryTime <= DateTime.UtcNow)
                    return BadRequest("Invalid refresh token");

                var newAccessToken = authService.GenerateToken(email, role, id);
                var newRefreshToken = authService.GenerateRefreshToken();

                admin.RefreshToken = newRefreshToken;
                await context.SaveChangesAsync();

                return Ok(new TokenModelDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken, Role = role });
            }
            else if (role == "Member")
            {
                var member = await context.Members.FindAsync(id);
                if (member == null || member.RefreshToken != refreshToken || member.RefreshTokenExpiryTime <= DateTime.UtcNow)
                    return BadRequest("Invalid refresh token");

                var newAccessToken = authService.GenerateToken(email, role, id);
                var newRefreshToken = authService.GenerateRefreshToken();

                member.RefreshToken = newRefreshToken;
                await context.SaveChangesAsync();

                return Ok(new TokenModelDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken, Role = role });
            }

            return BadRequest("Invalid request");
        }
    }


}
