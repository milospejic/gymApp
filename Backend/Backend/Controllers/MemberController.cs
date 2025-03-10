using AutoMapper;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        [Authorize(Roles = "Admin,User")]
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
            string here = "1";
            try
            {
                var membership = await membershipRepository.CreateMembership(memberDto.Membership);
                here = "2";
                var member = await memberRepository.CreateMember(memberDto, membership.MembershipID);
                here = "3";
                return Ok(member);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + here);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMember(Guid id, MemberUpdateDto memberDto)
        {
            try
            {
                await memberRepository.UpdateMember(id, memberDto);
                return Ok("Successfully updated member!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMember(Guid id)
        {

            try
            {
                var member = await memberRepository.GetMemberById(id);
                if (member == null)
                {
                    return NotFound("There is no member with id: " + id);
                }
                await memberRepository.DeleteMember(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeMemberPassword(Guid id, PasswordUpdateDto passwordUpdateDto)
        {
            try
            {
                await memberRepository.ChangeMemberPassword(id, passwordUpdateDto);
                return Ok("Password updated!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
