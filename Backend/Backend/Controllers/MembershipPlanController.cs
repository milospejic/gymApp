using AutoMapper;
using Azure.Messaging;
using Backend.Data.IRepository;
using Backend.Data.Repository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/membershipPlan")]
    [ApiController]
    public class MembershipPlanController : ControllerBase
    {
        private readonly IMembershipPlanRepository membershipPlanRepository;
        private readonly ILogger<MembershipPlanController> logger;

        public MembershipPlanController(IMembershipPlanRepository membershipPlanRepository, ILogger<MembershipPlanController> logger)
        {
            this.membershipPlanRepository = membershipPlanRepository;
            this.logger = logger;
        }


        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MembershipPlanDto>>> GetAllMembershipPlans()
        {
            logger.LogInformation("Fetching all membership plans.");
            try
            {
                var membershipPlans = await membershipPlanRepository.GetAllMembershipPlans();
                if (membershipPlans == null || !membershipPlans.Any())
                {
                    return NoContent();
                }
                return Ok(membershipPlans);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching all membership plans.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving all membership plans.");
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MembershipPlanDto>> GetMembershipPlanById(Guid id)
        {
            try
            {

                logger.LogInformation("Fetching membershipPlan with ID: {MembershipPlanId}", id);
                var membershipPlan = await membershipPlanRepository.GetMembershipPlanById(id);
                if (membershipPlan == null)
                {
                    logger.LogWarning("No membershipPlan found with ID: {MembershipPlanId}", id);
                    return NotFound($"No membershipPlan found with ID: {id}");
                }

                return Ok(membershipPlan);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving membershipPlan with ID: {MembershipPlanId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the membership plan.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> CreateMembershipPlan(MembershipPlanCreateDto membershipPlanDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                logger.LogWarning("Validation failed for MembershiPlanCreateDto: {Errors}", string.Join(" | ", errors));

                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }
            try
            {
                if (membershipPlanDto == null)
                {
                    logger.LogWarning("Attempt to create a membership plan with invalid data.");
                    return BadRequest("Invalid request data.");
                }

                var authenticatedAdminId = GetAuthenticatedAdminId();
                if (authenticatedAdminId == null)
                {
                    logger.LogWarning("Unauthorized create attempt.");
                    return Unauthorized("Invalid token");
                }

                logger.LogInformation("Creating a new membership plan.");
                var membershipPlan = await membershipPlanRepository.CreateMembershipPlan(membershipPlanDto, authenticatedAdminId);
                logger.LogInformation("Membership plan created successfully with ID: {MembershipPlanId}", membershipPlan.PlanID);
                return CreatedAtAction(nameof(GetMembershipPlanById), new { id = membershipPlan.PlanID }, membershipPlan);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "DbUpdateException occurred while creating membership plan. Inner Exception: {InnerException}", ex.InnerException?.Message);
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "ArgumentException when creating membership plan.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating a new membership plan.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the membership plan.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMembershipPlan(Guid id, MembershipPlanUpdateDto membershipPlanDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                logger.LogWarning("Validation failed for MembershipPlanUpdateDto: {Errors}", string.Join(" | ", errors));

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

                logger.LogInformation("Updating membership plan with ID: {MembershipPlanId}", id);
                await membershipPlanRepository.UpdateMembershipPlan(id, membershipPlanDto, authenticatedAdminId);
                return Ok("Successfully updated membership plan!");
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "DbUpdateException occurred while updating membership plan. Inner Exception: {InnerException}", ex.InnerException?.Message);
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "ArgumentException when updating membership plan.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating membership plan.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the membership plan.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMembershipPlan(Guid id)
        {

            try
            {
                logger.LogInformation("Deleting membership plan with ID: {MembershipPlanId}", id);
                await membershipPlanRepository.DeleteMembershipPlan(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "ArgumentException error when deleting membership plan.");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(ex, "InvalidOperation error when deleting membership plan.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error deleting membership plan.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the membership plan.");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("setForDeletion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetForDeletion(Guid id)
        {
            try
            {
                logger.LogInformation("Preparing membership plan with ID: {MembershipPlanId} for deletion", id);
                await membershipPlanRepository.SetPlanFordDeletion(id);
                return Ok("Successfully set membership plan for deletion!");
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Validation error when preparing membership plan for deletion.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error preparing membership plan for deletion..");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while preparing membership plan for deletion.");
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
