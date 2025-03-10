using AutoMapper;
using Azure.Messaging;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/membershipPlan")]
    [ApiController]
    public class MembershipPlanController : ControllerBase
    {
        private readonly IMembershipPlanRepository membershipPlanRepository;
        private readonly IMapper mapper;

        public MembershipPlanController(IMembershipPlanRepository membershipPlanRepository, IMapper mapper)
        {
            this.membershipPlanRepository = membershipPlanRepository;
            this.mapper = mapper;
        }


        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<MembershipPlanDto>>> GetAllMembershipPlans()
        {
            var membershipPlans = await membershipPlanRepository.GetAllMembershipPlans();
            if (membershipPlans == null || membershipPlans.Count() == 0)
            {
                return NoContent();
            }
            return Ok(membershipPlans);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MembershipPlanDto>> GetMembershipPlanById(Guid id)
        {
            var membershipPlan = await membershipPlanRepository.GetMembershipPlanById(id);
            if (membershipPlan == null)
            {
                return NotFound("There is no membershipPlan with id: " + id);
            }

            return Ok(membershipPlan);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> CreateMembershipPlan(MembershipPlanCreateDto membershipPlanDto)
        {
            var membershipPlanId = await membershipPlanRepository.CreateMembershipPlan(membershipPlanDto);
            return Ok(membershipPlanId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMembershipPlan(Guid id, MembershipPlanUpdateDto membershipPlanDto)
        {
            try
            {
                await membershipPlanRepository.UpdateMembershipPlan(id, membershipPlanDto);
                return Ok("Successfully updated membership plan!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMembershipPlan(Guid id)
        {

            try
            {
                await membershipPlanRepository.DeleteMembershipPlan(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
