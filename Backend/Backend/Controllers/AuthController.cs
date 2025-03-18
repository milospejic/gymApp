using Backend.AuthHelp;
using Microsoft.AspNetCore.Mvc;
using Backend.Dto.BasicDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Backend.Utils.CustomExceptions;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">ervice for handling authentication logic.</param>
        /// <param name="logger">Logger for capturing controller activity.</param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.logger = logger;
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
                logger.LogInformation("Admin login successful for email: {Email}", admin.AdminEmail);
                return Ok(new { Token = token, Role = "Admin" });
            }

            var member = await authService.IsMember(loginDto);
            if (member != null)
            {
                var token = authService.GenerateToken(member.MemberEmail, "Member", member.MemberId);
                logger.LogInformation("Member login successful for email: {Email}", member.MemberEmail);
                return Ok(new { Token = token, Role = "Member" });
            }

             throw new UnauthorizedAccessException("Invalid credentials");
        }
    }


}
