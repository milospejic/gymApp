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

            var membershipPlans = await membershipPlanRepository.GetAllMembershipPlans();
            if (membershipPlans == null || !membershipPlans.Any())
            {
                return NoContent();
            }
            return Ok(membershipPlans);
        }

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
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("setForDeletion/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetForDeletion(Guid id)
        {
            logger.LogInformation("Preparing membership plan with ID: {MembershipPlanId} for deletion", id);
            await membershipPlanRepository.SetPlanForDeletion(id);
            return Ok("Successfully set membership plan for deletion!");
        }
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
