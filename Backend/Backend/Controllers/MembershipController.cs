using AutoMapper;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/membership")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipRepository membershipRepository;
        private readonly IMapper mapper;

        public MembershipController(IMembershipRepository membershipRepository, IMapper mapper)
        {
            this.membershipRepository = membershipRepository;
            this.mapper = mapper;
        }


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

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMembership(Guid id, MembershipUpdateDto membershipDto)
        {
            try
            {
                await membershipRepository.UpdateMembership(id, membershipDto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
