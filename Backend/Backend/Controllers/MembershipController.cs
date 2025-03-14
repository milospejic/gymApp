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
    /// <summary>
    /// Controller responsible for managing membership-related operations,
    /// such as retrieving and updating memberships.
    /// </summary>
    [Route("api/membership")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipRepository membershipRepository;
        private readonly IMemberRepository memberRepository;
        private readonly ILogger<MembershipController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipController"/> class.
        /// </summary>
        /// <param name="membershipRepository">Service for handling membership-related database operations.</param>
        /// <param name="memberRepository">Service for handling member-related database operations.</param>
        /// <param name="logger">Logger for capturing controller activity.</param>
        public MembershipController(IMembershipRepository membershipRepository,IMemberRepository memberRepository,ILogger<MembershipController> logger)
        {
            this.membershipRepository = membershipRepository;
            this.memberRepository = memberRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all memberships in the system.
        /// Only accessible by authenticated admins.
        /// </summary>
        /// <returns>A list of memberships or NoContent if none exist.</returns>
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

        /// <summary>
        /// Retrieves the membership's details by their ID.
        /// Only accessible by authenticated admins and members.
        /// </summary>
        /// <param name="id">The unique identifier of the membership.</param>
        /// <returns>The details of the membership with the provided ID.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the user is not authenticated or lacks appropriate role.
        /// - 403 Forbidden: If a member attempts to access anothers membership data.
        /// - 404 Not Found: If no membership is found with the provided ID.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
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


        /// <summary>
        /// Updates the authenticated members's membership information.
        /// Only accessible by authenticated members.
        /// </summary>
        /// <param name="membershipDto">The membership update request containing necessary details.</param>
        /// <returns>A success response if the update is completed.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If the request data is invalid.
        /// - 401 Unauthorized: If the member is not authenticated.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
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

        /// <summary>
        /// Retrieves the authenticated member's ID from claims.
        /// </summary>
        /// <returns> MemberID if authenticated, otherwise null.</returns>
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
