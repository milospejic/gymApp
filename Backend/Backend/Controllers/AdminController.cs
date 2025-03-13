using AutoMapper;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Dto.UpdateDtos.Backend.Dto.UpdateDtos;
using Backend.Utils.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository adminRepository;
        private readonly ILogger<AdminController> logger;

        public AdminController(IAdminRepository adminRepository, ILogger<AdminController> logger)
        {
            this.adminRepository = adminRepository;
            this.logger = logger;
        }

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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
            logger.LogInformation("Creating a new admin.");
            var admin= await adminRepository.CreateAdmin(adminDto);
            logger.LogInformation("Admin created successfully with ID: {AdminId}", admin.AdminId);
            return CreatedAtAction(nameof(GetAdminById), new { id = admin.AdminId }, admin);
        }

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
            return NoContent();
        }

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
