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
    [Route("api/member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository memberRepository;
        private readonly IMembershipRepository membershipRepository;
        private readonly IAdminRepository adminRepository;
        private readonly ILogger<MemberController> logger;

        public MemberController(IMemberRepository memberRepository, IMembershipRepository membershipRepository, IAdminRepository adminRepository, ILogger<MemberController> logger)
        {
            this.memberRepository = memberRepository;
            this.membershipRepository = membershipRepository;
            this.adminRepository = adminRepository;
            this.logger = logger;
        }

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

        [Authorize(Roles = "Admin")]
        [HttpGet("{membershipId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        [AllowAnonymous]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
