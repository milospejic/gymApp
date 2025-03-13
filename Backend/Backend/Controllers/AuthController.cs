using Backend.AuthHelp;
using Microsoft.AspNetCore.Mvc;
using Backend.Dto.BasicDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Backend.Utils.CustomExceptions;

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
                throw new BadRequestException($"Validation failed for LoginDto: {string.Join(" | ", errors)}");

            }
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

             throw new UnauthorizedAccessException("Invalid credentials");
        }
    }


}
