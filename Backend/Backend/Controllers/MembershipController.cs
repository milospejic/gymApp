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
    [Route("api/membership")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipRepository membershipRepository;
        private readonly IMemberRepository memberRepository;
        private readonly IMapper mapper;

        public MembershipController(IMembershipRepository membershipRepository,IMemberRepository memberRepository, IMapper mapper)
        {
            this.membershipRepository = membershipRepository;
            this.memberRepository = memberRepository;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<MembershipDto>>> GetAllMemberships()
        {
            var memberships = await membershipRepository.GetAllMemberships();
            if (memberships == null || memberships.Count() == 0)
            {
                return NoContent();
            }
            return Ok(memberships);
        }

        [Authorize(Roles = "Admin,Member")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MembershipDto>> GetMembershipById(Guid id)
        {
            var membership = await membershipRepository.GetMembershipById(id);
            if (membership == null)
            {
                return NotFound("There is no membership with id: " + id);
            }

            return Ok(membership);
        }


        [Authorize(Roles = "Member")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMembership([FromBody] MembershipUpdateDto membershipDto)
        {
            try
            {
                var authenticatedMemberId = GetAuthenticatedUserId();
                if (authenticatedMemberId == null)
                    return Unauthorized("Invalid token");

                var member = await memberRepository.GetMemberById(authenticatedMemberId);

                await membershipRepository.UpdateMembership(member.MembershipId, membershipDto);
                return Ok("Successfully updated membership!");
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
