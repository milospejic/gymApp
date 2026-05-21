using Backend.AuthHelp;
using Backend.Dto.BasicDtos;
using Backend.Utils.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    /// <summary>
    /// Controller responsible for managing authentication-related operations, 
    /// such as logging in a user and refreshing expired access tokens.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// Notice we removed MyDbContext completely from here!
        /// </summary>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates the user based on the provided login credentials and generates access/refresh tokens.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

            var tokenModel = await _authService.AuthenticateAsync(loginDto);
            if (tokenModel == null)
            {
                _logger.LogWarning("Unauthorized login attempt for email: {Email}", loginDto.Email);
                return Unauthorized("Invalid email or password.");
            }

            _logger.LogInformation("Login successful for email: {Email} with Role: {Role}", loginDto.Email, tokenModel.Role);
            return Ok(tokenModel);
        }

        /// <summary>
        /// Validates an expired access token alongside a valid refresh token to rotate keys.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Refresh([FromBody] TokenModelDto tokenModel)
        {
            if (tokenModel == null || string.IsNullOrEmpty(tokenModel.AccessToken) || string.IsNullOrEmpty(tokenModel.RefreshToken))
            {
                return BadRequest("Invalid client request: Access and Refresh tokens are required.");
            }

            _logger.LogInformation("Refresh token rotation requested.");

            var newTokens = await _authService.RefreshTokensAsync(tokenModel);
            if (newTokens == null)
            {
                _logger.LogWarning("Token rotation failed: Invalid or expired refresh token token configuration.");
                return BadRequest("Invalid refresh token or access token token payload.");
            }

            return Ok(newTokens);
        }
    }
}