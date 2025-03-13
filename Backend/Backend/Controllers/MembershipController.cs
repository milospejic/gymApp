using AutoMapper;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Utils.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/membership")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipRepository membershipRepository;
        private readonly IMemberRepository memberRepository;
        private readonly ILogger<MembershipController> logger;

        public MembershipController(IMembershipRepository membershipRepository,IMemberRepository memberRepository,ILogger<MembershipController> logger)
        {
            this.membershipRepository = membershipRepository;
            this.memberRepository = memberRepository;
            this.logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MembershipDto>>> GetAllMemberships()
        {
            logger.LogInformation("Fetching all memberships.");
            var memberships = await membershipRepository.GetAllMemberships();
            if (memberships == null || !memberships.Any())
            {
                return NoContent();
            }
            return Ok(memberships);
        }

        [Authorize(Roles = "Admin,Member")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MembershipDto>> GetMembershipById(Guid id)
        {
            var authenticatedUserId = GetAuthenticatedUserId();
            if (authenticatedUserId == null)
            {
                throw new UnauthorizedAccessException($"Unauthorized access attempt to membership ID: {id}");
            }
            var member = await memberRepository.GetMemberByMembershipId(id);
            if (User.IsInRole("Member") && authenticatedUserId != member.MemberId)
            {
                logger.LogWarning("Forbidden access: Member {MemberId} attempted to access data of membership: {RequestedId}", authenticatedUserId, id);
                return Forbid("You can only access your own membership.");
            }
            logger.LogInformation("Fetching membership with ID: {MembershipId}", id);
            var membership = await membershipRepository.GetMembershipById(id);
                
            if (membership == null)
            {
                throw new NotFoundException($"No membership found with ID: {id}");
            }

            return Ok(membership);
        }


        [Authorize(Roles = "Member")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMembership([FromBody] MembershipUpdateDto membershipDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new BadRequestException($"Validation failed for MembershipUpdateDto: {string.Join(" | ", errors)}");

            }
            var authenticatedMemberId = GetAuthenticatedUserId();
            if (authenticatedMemberId == null)
            {
                throw new UnauthorizedAccessException("Unauthorized update attempt.");
            }
            var member = await memberRepository.GetMemberById(authenticatedMemberId);
            logger.LogInformation("Updating membership  with ID: {MembershipId}", member.MembershipId);
            await membershipRepository.UpdateMembership(member.MembershipId, membershipDto);
            return Ok("Successfully updated membership!");
        }


        private Guid? GetAuthenticatedUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                logger.LogWarning("No ClaimsIdentity found in the HTTP context.");
                return null;
            }

            var userIdClaim = identity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                logger.LogWarning("User ID claim not found in the claims.");
                return null;
            }

            logger.LogInformation("Extracted user ID claim: {UserIdClaim}", userIdClaim);

            if (Guid.TryParse(userIdClaim, out Guid authenticatedUserId))
            {
                logger.LogInformation("Successfully parsed user ID: {AuthenticatedUserId}", authenticatedUserId);
                return authenticatedUserId;
            }

            logger.LogWarning("Failed to parse user ID claim: {UserIdClaim}", userIdClaim);
            return null;
        }
    }
}
