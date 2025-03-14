using AutoMapper;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Utils.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Backend.Controllers
{
    /// <summary>
    /// Controller responsible for managing member-related operations,
    /// such as retrieving, creating, updating, and deleting member accounts.
    /// </summary>
    [Route("api/member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository memberRepository;
        private readonly IMembershipRepository membershipRepository;
        private readonly IAdminRepository adminRepository;
        private readonly ILogger<MemberController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberController"/> class.
        /// </summary>
        /// <param name="memberRepository">Service for handling member-related database operations.</param>
        /// <param name="membershipRepository">Service for handling membership-related database operations.</param>
        /// <param name="adminRepository">Service for handling admin-related database operations.</param>
        /// <param name="logger">Logger for capturing controller activity.</param>
        public MemberController(IMemberRepository memberRepository, IMembershipRepository membershipRepository, IAdminRepository adminRepository, ILogger<MemberController> logger)
        {
            this.memberRepository = memberRepository;
            this.membershipRepository = membershipRepository;
            this.adminRepository = adminRepository;
            this.logger = logger;
        }


        /// <summary>
        /// Retrieves a list of all members in the system.
        /// Only accessible by authenticated admins.
        /// </summary>
        /// <returns>A list of members or NoContent if none exist.</returns>
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
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers()
        {
            logger.LogInformation("Fetching all members.");

            var members = await memberRepository.GetAllMembers();
            if (members == null || !members.Any())
            {
                return NoContent();
            }
            return Ok(members);
        }

        /// <summary>
        /// Retrieves the information of the currently authenticated member.
        /// Only accessible by authenticated members.
        /// </summary>
        /// <returns>The details of the authenticated member.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the user is not authenticated.
        /// - 404 Not Found: If no member is found with the provided authenticated ID.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Member")]
        [HttpGet("myInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MemberDto>> GetMyInfo()
        {
            var authenticatedUserId = GetAuthenticatedUserId();
            if (authenticatedUserId == null)
            {
                throw new UnauthorizedAccessException("Unauthorized access attempt to 'myInfo'.");
            }

            logger.LogInformation("Fetching info for Member ID: {MemberId}", authenticatedUserId);
            var member = await memberRepository.GetMemberById(authenticatedUserId);
            if (member == null)
            {
                throw new NotFoundException($"No member found with ID: {authenticatedUserId}");
            }

            return Ok(member);
        }


        /// <summary>
        /// Retrieves the member's details by their ID.
        /// Only accessible by authenticated admins and members.
        /// </summary>
        /// <param name="id">The unique identifier of the member.</param>
        /// <returns>The details of the member with the provided ID.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the user is not authenticated or lacks appropriate role.
        /// - 403 Forgidden: If a member attempts to access anothers data.
        /// - 404 Not Found: If no member is found with the provided ID.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin,Member")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MemberDto>> GetMemberById(Guid id)
        {
            var authenticatedUserId = GetAuthenticatedUserId();
            if (authenticatedUserId == null)
            {
                throw new UnauthorizedAccessException($"Unauthorized access attempt to member ID: {id}");
            }

            if (User.IsInRole("Member") && authenticatedUserId != id)
            {
                logger.LogWarning("Forbidden access: Member {MemberId} attempted to access data of {RequestedId}", authenticatedUserId, id);
                return Forbid("You can only access your own data.");
            }

            logger.LogInformation("Fetching member with ID: {MemberId}", id);
            var member = await memberRepository.GetMemberById(id);
            if (member == null)
            {
                throw new NotFoundException($"No member found with ID: {id}");
            }

            return Ok(member);
        }

        /// <summary>
        /// Retrieves the member's details by their MembershipID.
        /// Only accessible by authenticated admins.
        /// </summary>
        /// <param name="membershipId">The unique identifier of the member's membership.</param>
        /// <returns>The details of the member with the provided MembershipId.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 401 Unauthorized: If the user is not authenticated or lacks appropriate role.
        /// - 404 Not Found: If no member is found with the provided membershipID.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpGet("membership")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MemberDto>> GetMemberByMembershipId(Guid membershipId)
        {
            logger.LogInformation("Fetching member with MembershipId: {MembershipId}", membershipId);
            var member = await memberRepository.GetMemberByMembershipId(membershipId);
            if (member == null)
            {
                throw new NotFoundException($"No member found with MembershipId: {membershipId}");
            }

            return Ok(member);
        }

        /// <summary>
        /// Only accessible by authenticated admins.
        /// Retrieves a member's details by their email address.
        /// </summary>
        /// <param name="email">The email address of the member.</param>
        /// <returns>The details of the member with the provided email.</returns>
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
        public async Task<ActionResult<MemberDto>> GetMemberByEmail(string email)
        {
            logger.LogInformation("Fetching member with email: {Email}", email);
            var member = await memberRepository.GetMemberByEmail(email);
            if (member == null)
            {
                throw new NotFoundException($"No member found with email: {email}");
            }

            return Ok(member);
        }

        /// <summary>
        /// Creates a new member account.
        /// Accesssible by all.
        /// </summary>
        /// <param name="memberDto">The member creation request containing necessary details.</param>
        /// <returns>The unique identifier of the created member.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If the provided data is invalid (e.g., missing or incorrect fields).
        /// - 409 Conflict: If the provided email address is already in use.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [AllowAnonymous]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> CreateMember(MemberCreateDto memberDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new BadRequestException($"Validation failed for MemberCreateDto: {string.Join(" | ", errors)}");

            }
            if(await memberRepository.GetMemberByEmail(memberDto.MemberEmail) != null && await adminRepository.GetAdminByEmail(memberDto.MemberEmail) != null)
            {
                throw new EmailAlreadyInUseException($"Email ({memberDto.MemberEmail}) is already taken");
            }
            logger.LogInformation("Creating a new member.");
            var membership = await membershipRepository.CreateMembership(memberDto.Membership);
            if (membership == null)
            {
                logger.LogError("Failed to create membership.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create membership.");
            }

            var member = await memberRepository.CreateMember(memberDto, membership.MembershipID);
            logger.LogInformation("Member created successfully with ID: {MemberId}", member.MemberId);
            return CreatedAtAction(nameof(GetMemberById), new { id = member.MemberId }, member);
        }

        /// <summary>
        /// Updates the authenticated member's profile information.
        /// Only accessible by authenticated members.
        /// </summary>
        /// <param name="memberDto">The member update request containing necessary details.</param>
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
        public async Task<IActionResult> UpdateMember([FromBody] MemberUpdateDto memberDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                throw new BadRequestException($"Validation failed for MemberuUpdateDto: {string.Join(" | ", errors)}");
            }
            var authenticatedUserId = GetAuthenticatedUserId();
            if (authenticatedUserId == null)
            {
                throw new UnauthorizedAccessException("Unauthorized update attempt.");
            }

            logger.LogInformation("Updating member with ID: {MemberId}", authenticatedUserId);
            await memberRepository.UpdateMember(authenticatedUserId, memberDto);
            return Ok("Successfully updated member!");
        }

        /// <summary>
        /// Deletes the currently authenticated member account.
        /// Only accessible by authenticated members.
        /// </summary>
        /// <returns>A success response if the account deletion is completed.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If there is an issue with the request.
        /// - 401 Unauthorized: If the member is not authenticated.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Member")]
        [HttpDelete] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMember()
        {
            var authenticatedUserId = GetAuthenticatedUserId();
            if (authenticatedUserId == null)
            {
                throw new UnauthorizedAccessException("Unauthorized delete attempt.");
            }
            logger.LogInformation("Deleting member with ID: {MemberId}", authenticatedUserId);
            await memberRepository.DeleteMember(authenticatedUserId);
            logger.LogInformation("Member with ID: {MemberId} deleted successfully", authenticatedUserId);
            return NoContent();
        }

        /// <summary>
        /// Changes the password for the authenticated member.
        /// Only accessible by authenticated members.
        /// </summary>
        /// <param name="passwordUpdateDto">The password update request containing the old and new password.</param>
        /// <returns>A success response if the password change is completed.</returns>
        /// <remarks>
        /// Possible errors:
        /// - 400 Bad Request: If the provided data is invalid.
        /// - 401 Unauthorized: If the member is not authenticated.
        /// - 500 Internal Server Error: If an unexpected error occurs during processing.
        /// </remarks>
        [Authorize(Roles = "Member")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeMemberPassword([FromBody] PasswordUpdateDto passwordUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                throw new BadRequestException($"Validation failed for PasswordUpdateDto: {string.Join(" | ", errors)}");

            }
            var authenticatedUserId = GetAuthenticatedUserId();
            if (authenticatedUserId == null)
            {
                throw new UnauthorizedAccessException("Unauthorized access attempt to change password.");

            }

            logger.LogInformation("Changing password for member with ID: {MemberId}", authenticatedUserId);

            await memberRepository.ChangeMemberPassword(authenticatedUserId, passwordUpdateDto);

            logger.LogInformation("Password successfully updated for member with ID: {MemberId}", authenticatedUserId);
            return Ok("Password updated!");
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
