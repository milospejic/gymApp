using Backend.AuthHelp;
using Microsoft.AspNetCore.Mvc;
using Backend.Dto.BasicDtos;
using Microsoft.AspNetCore.Identity;

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

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (authService.IsAdmin(loginDto).Result != false || authService.IsMember(loginDto).Result != false)
            {

                var role = authService.IsAdmin(loginDto).Result ? "Admin" : "Member";
                var token = authService.GenerateToken(loginDto.Email, role);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }
    }

    
}
