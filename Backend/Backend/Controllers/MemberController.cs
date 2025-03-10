using AutoMapper;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository memberRepository;
        private readonly IMembershipRepository membershipRepository;
        private readonly IMapper mapper;

        public MemberController(IMemberRepository memberRepository, IMembershipRepository membershipRepository, IMapper mapper)
        {
            this.memberRepository = memberRepository;
            this.membershipRepository = membershipRepository;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers()
        {
            var members = await memberRepository.GetAllMembers();
            if (members == null || members.Count() == 0)
            {
                return NoContent();
            }
            return Ok(members);
        }


        [Authorize(Roles = "Admin,Member")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberDto>> GetMemberById(Guid id)
        {
            var member = await memberRepository.GetMemberById(id);
            if (member == null)
            {
                return NotFound("There is no member with id: " + id);
            }

            return Ok(member);
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> CreateMember(MemberCreateDto memberDto)
        {
            try
            {
                var membership = await membershipRepository.CreateMembership(memberDto.Membership);
                var member = await memberRepository.CreateMember(memberDto, membership.MembershipID);
                return Ok(member);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }


        [Authorize(Roles = "Member")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMember([FromBody] MemberUpdateDto memberDto)
        {
            try
            {
                var authenticatedUserId = GetAuthenticatedUserId();
                if (authenticatedUserId == null)
                    return Unauthorized("Invalid token");

                await memberRepository.UpdateMember(authenticatedUserId, memberDto);
                return Ok("Successfully updated member!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Member")]
        [HttpDelete] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMember()
        {
            try
            {
                var authenticatedUserId = GetAuthenticatedUserId();
                if (authenticatedUserId == null)
                    return Unauthorized("Invalid token");

                var member = await memberRepository.GetMemberById(authenticatedUserId);
                if (member == null)
                {
                    return NotFound($"There is no member with id: {authenticatedUserId}");
                }

                await memberRepository.DeleteMember(authenticatedUserId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Member")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeMemberPassword([FromBody] PasswordUpdateDto passwordUpdateDto)
        {
            try
            {
                var authenticatedUserId = GetAuthenticatedUserId();
                if (authenticatedUserId == null)
                    return Unauthorized("Invalid token");

                await memberRepository.ChangeMemberPassword(authenticatedUserId, passwordUpdateDto);
                return Ok("Password updated!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

   
        private Guid? GetAuthenticatedUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = identity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out Guid authenticatedUserId))
            {
                return authenticatedUserId;
            }

            return null;
        }

    }
}
