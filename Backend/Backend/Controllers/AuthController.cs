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

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var admin = await authService.IsAdmin(loginDto);
            if (admin != null)
            {
                var token = authService.GenerateToken(admin.AdminEmail, "Admin", admin.AdminId);
                return Ok(new { Token = token });
            }

            var member = await authService.IsMember(loginDto);
            if (member != null)
            {
                var token = authService.GenerateToken(member.MemberEmail, "Member", member.MemberId);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }


    }


}
