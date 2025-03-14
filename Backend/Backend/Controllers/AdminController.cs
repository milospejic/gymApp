using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Utils.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    /// <summary>
    /// Controller responsible for managing administrator-related operations,
    /// such as retrieving, creating, updating, and deleting admin accounts.
    /// Requires Admin role authorization for all actions.
    /// </summary>
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository adminRepository;
        private readonly IMemberRepository memberRepository;
        private readonly ILogger<AdminController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="adminRepository">Service for handling admin-related database operations.</param>
        /// <param name="memberRepository">Service for handling member-related database operations.</param>
        /// <param name="logger">Logger for capturing controller activity.</param>
        public AdminController(IAdminRepository adminRepository, IMemberRepository memberRepository, ILogger<AdminController> logger)
        {
            this.adminRepository = adminRepository;
            this.memberRepository = memberRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all administrators in the system.
        /// </summary>
        /// <returns>A list of administrators or NoContent if none exist.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the user is not authenticated or lacks appropriate role.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetAllAdmins()
        {
            logger.LogInformation("Fetching all admins.");
            var admins = await adminRepository.GetAllAdmins();
            if (admins == null || !admins.Any())
            {
                return NoContent();
            }
            return Ok(admins);
        }

        /// <summary>
        /// Retrieves the information of the currently authenticated administrator.
        /// </summary>
        /// <returns>The details of the authenticated administrator.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the user is not authenticated.
        /// - 404 Not Found: If no admin is found with the provided authenticated ID.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpGet("myInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AdminDto>> GetMyInfo()
        {
            var authenticatedAdminId = GetAuthenticatedAdminId();
            if (authenticatedAdminId == null)
            {
                throw new UnauthorizedAccessException("Unauthorized access attempt to 'myInfo'.");
            }
            logger.LogInformation("Fetching info for Admin ID: {AdminId}", authenticatedAdminId);
            var admin = await adminRepository.GetAdminById(authenticatedAdminId.Value);
            if (admin == null)
            {
                throw new NotFoundException($"No admin found with ID: {authenticatedAdminId}");
            }
             return Ok(admin);
        }

        /// <summary>
        /// Retrieves the administrator's details by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the administrator.</param>
        /// <returns>The details of the administrator with the provided ID.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the user is not authenticated or lacks appropriate role.
        /// - 404 Not Found: If no admin is found with the provided ID.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AdminDto>> GetAdminById(Guid id)
        {
            logger.LogInformation("Fetching admin with ID: {AdminId}", id);
            var admin = await adminRepository.GetAdminById(id);
            if (admin == null)
             {
                throw new NotFoundException($"No admin found with ID: {id}");
            }
            return Ok(admin);
        }

        /// <summary>
        /// Retrieves an administrator's details by their email address.
        /// </summary>
        /// <param name="email">The email address of the administrator.</param>
        /// <returns>The details of the administrator with the provided email.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the user is not authenticated or lacks appropriate role.
        /// - 404 Not Found: If no admin is found with the provided email.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpGet("email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MemberDto>> GetAdminByEmail(string email)
        {
            logger.LogInformation("Fetching member with email: {Email}", email);
            var admin = await adminRepository.GetAdminByEmail(email);
            if (admin == null)
            {
                throw new NotFoundException($"No admin found with email: {email}");
            }

            return Ok(admin);
        }

        /// <summary>
        /// Creates a new administrator account.
        /// </summary>
        /// <param name="adminDto">The admin creation request containing necessary details.</param>
        /// <returns>The unique identifier of the created administrator.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If the provided data is invalid (e.g., missing or incorrect fields).
        /// - 401 Unauthorized: If the user is not authenticated or lacks appropriate role.
        /// - 409 Conflict: If the provided email address is already in use.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> CreateAdmin(AdminCreateDto adminDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new BadRequestException($"Validation failed for AdminCreateDto: {string.Join(" | ", errors)}");
            }
            if (await memberRepository.GetMemberByEmail(adminDto.AdminEmail) != null || await adminRepository.GetAdminByEmail(adminDto.AdminEmail) != null)
            {
                throw new EmailAlreadyInUseException($"Email ({adminDto.AdminEmail}) is already taken");
            }
            logger.LogInformation("Creating a new admin.");
            var admin= await adminRepository.CreateAdmin(adminDto);
            logger.LogInformation("Admin created successfully with ID: {AdminId}", admin.AdminId);
            return CreatedAtAction(nameof(GetAdminById), new { id = admin.AdminId }, admin);
        }

        /// <summary>
        /// Updates the authenticated administrator's profile information.
        /// </summary>
        /// <param name="adminDto">The admin update request containing necessary details.</param>
        /// <returns>A success response if the update is completed.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If the request data is invalid.
        /// - 401 Unauthorized: If the administrator is not authenticated.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAdmin([FromBody] AdminUpdateDto adminDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new BadRequestException($"Validation failed for AdminUpdateDto: {string.Join(" | ", errors)}");

            }
            var authenticatedAdminId = GetAuthenticatedAdminId();
            if (authenticatedAdminId == null)
            {
                throw new UnauthorizedAccessException("Unauthorized update attempt.");
            }
            logger.LogInformation("Updating admin with ID: {AdminId}", authenticatedAdminId);
            await adminRepository.UpdateAdmin(authenticatedAdminId, adminDto);
            return Ok("Successfully updated admin!");    
        }

        /// <summary>
        /// Deletes the currently authenticated administrator account.
        /// </summary>
        /// <returns>A success response if the account deletion is completed.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If there is an issue with the request.
        /// - 401 Unauthorized: If the administrator is not authenticated.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAdmin()
        {
            var authenticatedAdminId = GetAuthenticatedAdminId();
            if (authenticatedAdminId == null)
            {
                throw new UnauthorizedAccessException("Unauthorized delete attempt.");
            }
            logger.LogInformation("Deleting admin with ID: {AdminId}", authenticatedAdminId);
            await adminRepository.DeleteAdmin(authenticatedAdminId);
            logger.LogInformation("Admin with ID: {AdminId} deleted successfully", authenticatedAdminId);
            return NoContent();
        }

        /// <summary>
        /// Changes the password for the authenticated administrator.
        /// </summary>
        /// <param name="passwordUpdateDto">The password update request containing the old and new password.</param>
        /// <returns>A success response if the password change is completed.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If the provided data is invalid.
        /// - 401 Unauthorized: If the administrator is not authenticated.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeAdminPassword([FromBody] PasswordUpdateDto passwordUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new BadRequestException($"Validation failed for PasswordUpdateDto: {string.Join(" | ", errors)}");

            }
            var authenticatedAdminId = GetAuthenticatedAdminId();
            if (authenticatedAdminId == null)
            {
                throw new UnauthorizedAccessException("Unauthorized access attempt to change password.");
            }
            logger.LogInformation("Changing password for admin with ID: {AdminId}", authenticatedAdminId);
            await adminRepository.ChangeAdminPassword(authenticatedAdminId, passwordUpdateDto);
            logger.LogInformation("Password successfully updated for admin with ID: {AdminId}", authenticatedAdminId);
            return Ok("Password updated!");
        }

        /// <summary>
        /// Retrieves the authenticated admin's ID from claims.
        /// </summary>
        /// <returns>Admin ID if authenticated, otherwise null.</returns>
        private Guid? GetAuthenticatedAdminId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                logger.LogWarning("No ClaimsIdentity found in the HTTP context.");
                return null;
            }
            var adminIdClaim = identity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(adminIdClaim))
            {
                logger.LogWarning("Admin ID claim not found in the claims.");
                return null;
            }

            logger.LogInformation("Extracted admin ID claim: {AdminIdClaim}", adminIdClaim);

            if (Guid.TryParse(adminIdClaim, out Guid authenticatedAdminId))
            {
                logger.LogInformation("Successfully parsed admin ID: {AuthenticatedAdminId}", authenticatedAdminId);
                return authenticatedAdminId;
            }

            logger.LogWarning("Failed to parse admin ID claim: {AdminIdClaim}", adminIdClaim);
            return null;
        }

    }
}
