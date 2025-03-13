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
        private readonly ILogger<MemberController> logger;

        public MemberController(IMemberRepository memberRepository, IMembershipRepository membershipRepository, IMapper mapper, ILogger<MemberController> logger)
        {
            this.memberRepository = memberRepository;
            this.membershipRepository = membershipRepository;
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
            /*try
            {*/
                var members = await memberRepository.GetAllMembers();
                if (members == null || !members.Any())
                {
                    return NoContent();
                }
                return Ok(members);
            /*}
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching all members.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving all members.");
            }*/
        }

        [Authorize(Roles = "Member")]
        [HttpGet("myInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MemberDto>> GetMyInfo()
        {
            /*try
            {*/
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
            /*}
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving member info.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving your information.");
            }*/
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
            /*try
            {*/
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
            /*}
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving member with ID: {MemberId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the member.");
            }*/
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
            /*try
            {*/

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
            /*}
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "DbUpdateException occurred while creating member. Inner Exception: {InnerException}", ex.InnerException?.Message);
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "ArgumentException occurred while creating member.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating a new member.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the member.");
            }*/
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
            /*try
            {*/
                var authenticatedUserId = GetAuthenticatedUserId();
                if (authenticatedUserId == null)
                {
                    throw new UnauthorizedAccessException("Unauthorized update attempt.");
                }

                logger.LogInformation("Updating member with ID: {MemberId}", authenticatedUserId);
                await memberRepository.UpdateMember(authenticatedUserId, memberDto);
                return Ok("Successfully updated member!");
            /*}
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "DbUpdateException occurred while creating member. Inner Exception: {InnerException}", ex.InnerException?.Message);
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "ArgumentException occurred while updating member.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating member.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the member.");
            }*/
        }

        [Authorize(Roles = "Member")]
        [HttpDelete] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMember()
        {
            /*try
            {*/
                var authenticatedUserId = GetAuthenticatedUserId();
                if (authenticatedUserId == null)
                {
                    throw new UnauthorizedAccessException("Unauthorized delete attempt.");
                }

                logger.LogInformation("Deleting member with ID: {MemberId}", authenticatedUserId);
                await memberRepository.DeleteMember(authenticatedUserId);

                return NoContent();
           /* }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "ArgumentException occurred while deleting member.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting member.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the member.");
            }*/
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
            /*try
            {*/
                var authenticatedUserId = GetAuthenticatedUserId();
                if (authenticatedUserId == null)
                {
                    throw new UnauthorizedAccessException("Unauthorized access attempt to change password.");

                }

                logger.LogInformation("Changing password for member with ID: {MemberId}", authenticatedUserId);

                await memberRepository.ChangeMemberPassword(authenticatedUserId, passwordUpdateDto);

                logger.LogInformation("Password successfully updated for member with ID: {MemberId}", authenticatedUserId);
                return Ok("Password updated!");
           /* }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "ArgumentException occurred while changing password.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while changing password for member.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while changing the password.");
            }*/
        }



        private Guid? GetAuthenticatedUserId()
        {
            /*try
            {*/
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
            /*}
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving authenticated user ID.");
                return null;
            }*/
        }


    }
}
