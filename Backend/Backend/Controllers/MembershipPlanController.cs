using AutoMapper;
using Azure.Messaging;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var authenticatedAdminId = GetAuthenticatedAdminId();
            if (authenticatedAdminId == null)
                return Unauthorized("Invalid token");
            var membershipPlanId = await membershipPlanRepository.CreateMembershipPlan(membershipPlanDto, authenticatedAdminId);
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
                var authenticatedAdminId = GetAuthenticatedAdminId();
                if (authenticatedAdminId == null)
                    return Unauthorized("Invalid token");
                await membershipPlanRepository.UpdateMembershipPlan(id, membershipPlanDto, authenticatedAdminId);
                return Ok("Successfully updated membership plan!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }catch(InvalidOperationException ex2)
            {
                return BadRequest(ex2.Message);
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
            catch (InvalidOperationException ex2)
            {
                return BadRequest(ex2.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("setForDeletion")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetForDeletion(Guid id)
        {
            try
            {
                await membershipPlanRepository.SetPlanFordDeletion(id);
                return Ok("Successfully ser membership plan for deletion!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Guid? GetAuthenticatedAdminId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = identity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out Guid authenticatedAdminId))
            {
                return authenticatedAdminId;
            }

            return null;
        }
    }
}
