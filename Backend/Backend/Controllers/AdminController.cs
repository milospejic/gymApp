using AutoMapper;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Dto.UpdateDtos.Backend.Dto.UpdateDtos;
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
            try
            {
                var admins = await adminRepository.GetAllAdmins();
                if (admins == null || !admins.Any())
                {
                    return NoContent();
                }
                return Ok(admins);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching all admins.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving all admins.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("myInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AdminDto>> GetMyInfo()
        {
            try
            {
                var authenticatedAdminId = GetAuthenticatedAdminId();
                if (authenticatedAdminId == null)
                {
                    logger.LogWarning("Unauthorized access attempt to 'myInfo'.");
                    return Unauthorized("Invalid token");
                }

                logger.LogInformation("Fetching info for Admin ID: {AdminId}", authenticatedAdminId);
                var admin = await adminRepository.GetAdminById(authenticatedAdminId.Value);
                if (admin == null)
                {
                    logger.LogWarning("No admin found with ID: {MemberId}", authenticatedAdminId);
                    return NotFound($"No admin found with ID: {authenticatedAdminId}");
                }

                return Ok(admin);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving admin info.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving your information.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AdminDto>> GetAdminById(Guid id)
        {
            try
            {
                logger.LogInformation("Fetching admin with ID: {AdminId}", id);
                var admin = await adminRepository.GetAdminById(id);
                if (admin == null)
                {
                    logger.LogWarning("No admin found with ID: {AdminId}", id);
                    return NotFound($"No admin found with ID: {id}");
                }

                return Ok(admin);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving admin with ID: {AdminId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the admin.");
            }
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

                logger.LogWarning("Validation failed for AdminCreateDto: {Errors}", string.Join(" | ", errors));

                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }
            try
            {
                if (adminDto == null)
                {
                    logger.LogWarning("Attempt to create a admin with invalid data.");
                    return BadRequest("Invalid request data.");
                }

                logger.LogInformation("Creating a new admin.");
                var admin= await adminRepository.CreateAdmin(adminDto);
                logger.LogInformation("Admin created successfully with ID: {AdminId}", admin.AdminId);
                return CreatedAtAction(nameof(GetAdminById), new { id = admin.AdminId }, admin);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "DbUpdateException occurred while creating admin. Inner Exception: {InnerException}", ex.InnerException?.Message);
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "ArgumentException when creating admin.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating a new admin.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the admin.");
            }
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

                logger.LogWarning("Validation failed for AdminUpdateDto: {Errors}", string.Join(" | ", errors));

                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }
            try
            {
                var authenticatedAdminId = GetAuthenticatedAdminId();
                if (authenticatedAdminId == null)
                {
                    logger.LogWarning("Unauthorized update attempt.");
                    return Unauthorized("Invalid token");
                }

                logger.LogInformation("Updating admin with ID: {AdminId}", authenticatedAdminId);
                await adminRepository.UpdateAdmin(authenticatedAdminId, adminDto);
                return Ok("Successfully updated admin!");
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "DbUpdateException occurred while updating admin. Inner Exception: {InnerException}", ex.InnerException?.Message);
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Validation error when updating admin.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating member.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the admin.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAdmin()
        {
            try
            {
                var authenticatedAdminId = GetAuthenticatedAdminId();
                if (authenticatedAdminId == null)
                {
                    logger.LogWarning("Unauthorized delete attempt.");
                    return Unauthorized("Invalid token");
                }

                logger.LogInformation("Deleting admin with ID: {AdminId}", authenticatedAdminId);

                await adminRepository.DeleteAdmin(authenticatedAdminId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "ArgumentException when deleting admin.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting admin.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the admin.");
            }
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

                logger.LogWarning("Validation failed for PasswordUpdateDto: {Errors}", string.Join(" | ", errors));

                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }
            try
            {
                var authenticatedAdminId = GetAuthenticatedAdminId();
                if (authenticatedAdminId == null)
                {
                    logger.LogWarning("Unauthorized access attempt to change password.");
                    return Unauthorized("Invalid token");
                }

                logger.LogInformation("Changing password for admin with ID: {AdminId}", authenticatedAdminId);

                await adminRepository.ChangeAdminPassword(authenticatedAdminId, passwordUpdateDto);
                logger.LogInformation("Password successfully updated for admin with ID: {AdminId}", authenticatedAdminId);
                return Ok("Password updated!");
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "ArgumentException occurred while changing password.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while changing password for admin.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while changing the password.");
            }
        }

        private Guid? GetAuthenticatedAdminId()
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving authenticated admin ID.");
                return null;
            }
        }

    }
}
