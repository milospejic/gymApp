using Backend.AuthHelp;
using Microsoft.AspNetCore.Mvc;
using Backend.Dto.BasicDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ILogger<AuthController> logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.logger = logger;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                logger.LogWarning("Validation failed for LoginDto: {Errors}", string.Join(" | ", errors));

                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }
            try
            {
                logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

                var admin = await authService.IsAdmin(loginDto);
                if (admin != null)
                {
                    var token = authService.GenerateToken(admin.AdminEmail, "Admin", admin.AdminId);
                    logger.LogInformation("Admin login successful for email: {Email}", admin.AdminEmail);
                    return Ok(new { Token = token });
                }

                var member = await authService.IsMember(loginDto);
                if (member != null)
                {
                    var token = authService.GenerateToken(member.MemberEmail, "Member", member.MemberId);
                    logger.LogInformation("Member login successful for email: {Email}", member.MemberEmail);
                    return Ok(new { Token = token });
                }

                logger.LogWarning("Invalid login attempt for email: {Email}", loginDto.Email);
                return Unauthorized("Invalid credentials");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during login for email: {Email}", loginDto.Email);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }


}
