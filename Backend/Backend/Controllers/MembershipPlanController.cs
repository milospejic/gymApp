using AutoMapper;
using Azure.Messaging;
using Backend.Data.IRepository;
using Backend.Data.Repository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;
using Backend.Utils.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Backend.Controllers
{
    /// <summary>
    /// Controller responsible for managing membershipPlan-related operations,
    /// such as retrieving, creating, updating, and deleting membership plans.
    /// </summary>
    [Route("api/membershipPlan")]
    [ApiController]
    public class MembershipPlanController : ControllerBase
    {
        private readonly IMembershipPlanRepository membershipPlanRepository;
        private readonly ILogger<MembershipPlanController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipPlanController"/> class.
        /// </summary>
        /// <param name="membershipPlanRepository">Service for handling membershipPlan-related database operations.</param>
        /// <param name="logger">Logger for capturing controller activity.</param>
        public MembershipPlanController(IMembershipPlanRepository membershipPlanRepository, ILogger<MembershipPlanController> logger)
        {
            this.membershipPlanRepository = membershipPlanRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all membershipPlans in the system.
        /// Accesssible by all.
        /// </summary>
        /// <returns>A list of membershipPlans or NoContent if none exist.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the user is not authenticated or lacks appropriate role.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MembershipPlanDto>>> GetAllMembershipPlans()
        {
            logger.LogInformation("Fetching all membership plans.");

            var membershipPlans = await membershipPlanRepository.GetAllMembershipPlans();
            if (membershipPlans == null || !membershipPlans.Any())
            {
                return NoContent();
            }
            return Ok(membershipPlans);
        }

        /// <summary>
        /// Retrieves the membershipPlan's details by their ID.
        /// Accesssible by all.
        /// </summary>
        /// <param name="id">The unique identifier of the membershipPlan.</param>
        /// <returns>The details of the membershipPlan with the provided ID.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the user is not authenticated or lacks appropriate role.
        /// - 404 Not Found: If no membershipPlan is found with the provided ID.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MembershipPlanDto>> GetMembershipPlanById(Guid id)
        {
            logger.LogInformation("Fetching membershipPlan with ID: {MembershipPlanId}", id);
            var membershipPlan = await membershipPlanRepository.GetMembershipPlanById(id);
            if (membershipPlan == null)
            {
                throw new NotFoundException($"No membershipPlan found with ID: {id}");
            }
            return Ok(membershipPlan);
        }

        /// <summary>
        /// Creates a new membershipPlan.
        /// Only accessible by authenticated admins.
        /// </summary>
        /// <param name="membershipPlanDto">The membershipPlan creation request containing necessary details.</param>
        /// <returns>The unique identifier of the created membershipPlan.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If the provided data is invalid (e.g., missing or incorrect fields).
        /// - 401 Unauthorized: If the user is not authenticated or lacks appropriate role.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
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

                throw new BadRequestException($"Validation failed for MembershiPlanCreateDto: {string.Join(" | ", errors)}");
            }
            var authenticatedAdminId = GetAuthenticatedAdminId();
            if (authenticatedAdminId == null)
            {
                throw new UnauthorizedAccessException("Unauthorized create attempt.");
            }

            logger.LogInformation("Creating a new membership plan.");
            var membershipPlan = await membershipPlanRepository.CreateMembershipPlan(membershipPlanDto, authenticatedAdminId);
            logger.LogInformation("Membership plan created successfully with ID: {MembershipPlanId}", membershipPlan.PlanID);
            return CreatedAtAction(nameof(GetMembershipPlanById), new { id = membershipPlan.PlanID }, membershipPlan);
        }

        /// <summary>
        /// Updates the membeshipPlan information.
        /// Only accessible by authenticated admins.
        /// </summary>
        /// <param name="id">The unique identifier of the membershipPlan.</param>
        /// <param name="membershipPlanDto">The membeshipPlan update request containing necessary details.</param>
        /// <returns>A success response if the update is completed.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If the request data is invalid.
        /// - 401 Unauthorized: If the administrator is not authenticated.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
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
                throw new BadRequestException($"Validation failed for MembershipPlanUpdateDto: {string.Join(" | ", errors)}");

            }
            var authenticatedAdminId = GetAuthenticatedAdminId();
            if (authenticatedAdminId == null)
            { 
                throw new UnauthorizedAccessException("Unauthorized update attempt.");
            }

            logger.LogInformation("Updating membership plan with ID: {MembershipPlanId}", id);
            await membershipPlanRepository.UpdateMembershipPlan(id, membershipPlanDto, authenticatedAdminId);
            return Ok("Successfully updated membership plan!");
        }

        /// <summary>
        /// Deletes the membershipPlan by their ID.
        /// Only accessible by authenticated admins.
        /// </summary>
        /// <param name="id">The unique identifier of the membershipPlan.</param>
        /// <returns>A success response if the account deletion is completed.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If there is an issue with the request.
        /// - 401 Unauthorized: If the administrator is not authenticated.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMembershipPlan(Guid id)
        {
            logger.LogInformation("Deleting membership plan with ID: {MembershipPlanId}", id);
            await membershipPlanRepository.DeleteMembershipPlan(id);
            logger.LogInformation("Membership plan with ID: {MembershipPlanId} deleted successfully", id);
            return NoContent();
        }

        /// <summary>
        /// Sets the ForDeletion property of the membershipPlan to true.
        /// Only accessible by authenticated admins.
        /// </summary>
        /// <param name="id">The unique identifier of the membershipPlan.</param>
        /// <returns>A success response if the membershipPlan was prepared for deletion.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the administrator is not authenticated.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPatch("setForDeletion/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetForDeletion(Guid id)
        {
            logger.LogInformation("Preparing membership plan with ID: {MembershipPlanId} for deletion", id);
            await membershipPlanRepository.SetPlanForDeletion(id);
            return Ok("Successfully set membership plan for deletion!");
        }

        /// <summary>
        /// Sets the ForDeletion property of the membershipPlan to false.
        /// Only accessible by authenticated admins.
        /// </summary>
        /// <param name="id">The unique identifier of the membershipPlan.</param>
        /// <returns>A success response if the membershipPlan was set active.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the administrator is not authenticated.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPatch("resetForDeletion/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetForDeletion(Guid id)
        {
            logger.LogInformation("Making membership plan with ID: {MembershipPlanId} active again", id);
            await membershipPlanRepository.SetPlanForDeletion(id);
            return Ok("Successfully set membership plan active!");
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
