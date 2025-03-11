using AutoMapper;
using Backend.Data.IRepository;
using Backend.Data.Repository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Dto.UpdateDtos.Backend.Dto.UpdateDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository adminRepository;
        private readonly IMapper mapper;

        public AdminController(IAdminRepository adminRepository, IMapper mapper)
        {
            this.adminRepository = adminRepository;
            this.mapper = mapper;
        }
       
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetAllAdmins()
        {
            var admins = await adminRepository.GetAllAdmins();
            if (admins == null || admins.Count() == 0)
            {
                return NoContent();
            }
            return Ok(admins);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("myInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMyInfo()
        {
            var authenticatedAdminId = GetAuthenticatedAdminId();
            if (authenticatedAdminId == null)
                return Unauthorized("Invalid token");
            var admin = await adminRepository.GetAdminById(authenticatedAdminId);
            if (admin == null)
            {
                return NotFound("There is no admin with id: " + authenticatedAdminId);
            }

            return Ok(admin);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AdminDto>> GetAdminById(Guid id)
        {
            var admin = await adminRepository.GetAdminById(id);
            if (admin == null)
            {
                return NotFound("There is no admin with id: " + id);
            }

            return Ok(admin);
        }
       
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> CreateAdmin(AdminCreateDto adminDto)
        {
            try
            {
                var admin = await adminRepository.CreateAdmin(adminDto);
                return Ok(admin);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       

      
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAdmin([FromBody] AdminUpdateDto adminDto)
        {
            try
            {
                var authenticatedAdminId = GetAuthenticatedAdminId();
                if (authenticatedAdminId == null)
                    return Unauthorized("Invalid token");

                await adminRepository.UpdateAdmin(authenticatedAdminId, adminDto);
                return Ok("Admin updated!");
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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAdmin()
        {
            try
            {
                var authenticatedAdminId = GetAuthenticatedAdminId();
                if (authenticatedAdminId == null)
                    return Unauthorized("Invalid token");

                await adminRepository.DeleteAdmin(authenticatedAdminId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeAdminPassword([FromBody] PasswordUpdateDto passwordUpdateDto)
        {
            try
            {
                var authenticatedAdminId = GetAuthenticatedAdminId();
                if (authenticatedAdminId == null)
                    return Unauthorized("Invalid token");

                await adminRepository.ChangeAdminPassword(authenticatedAdminId, passwordUpdateDto);
                return Ok("Password updated!");
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
