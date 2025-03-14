using AutoMapper;
using Backend.Data.Context;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repository
{
    /// <summary>
    /// Implements the <see cref="IMemberRepository"/> interface for managing member-related operations.
    /// </summary>
    public class MemberRepository : IMemberRepository
    {
        private readonly MyDbContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for querying and saving member data.</param>
        /// <param name="mapper">The AutoMapper instance used for mapping entities to DTOs.</param>
        public MemberRepository(MyDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Retrieves all members from the database.
        /// </summary>
        /// <returns>A collection of <see cref="MemberDto"/> representing all members.</returns>
        public async Task<IEnumerable<MemberDto>> GetAllMembers()
        {
            var members = await context.Members
                .Include(m => m.Membership)
                    .ThenInclude(ms => ms.MembershipPlan)
                    .ThenInclude(ma => ma.Admin)
                    .ToListAsync();
            return mapper.Map<IEnumerable<MemberDto>>(members);
        }

        /// <summary>
        /// Retrieves a member by their unique ID.
        /// </summary>
        /// <param name="id">The members's ID.</param>
        /// <returns>The <see cref="MemberDto"/> corresponding to the specified ID, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="id"/> is null.</exception>
        public async Task<MemberDto> GetMemberById(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }
            var member = await context.Members
                .Include(m => m.Membership)
                .ThenInclude(ms => ms.MembershipPlan)
                .ThenInclude(ma => ma.Admin)
                    .FirstOrDefaultAsync(m => m.MemberId == id);
            return mapper.Map<MemberDto>(member);
        }

        /// <summary>
        /// Retrieves a member by their membership ID.
        /// </summary>
        /// <param name="id">The membership ID.</param>
        /// <returns>The <see cref="MemberDto"/> corresponding to the specified membership ID, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="id"/> is null.</exception>
        public async Task<MemberDto> GetMemberByMembershipId(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }
            var member = await context.Members
                .Include(m => m.Membership)
                .ThenInclude(ms => ms.MembershipPlan)
                .ThenInclude(ma => ma.Admin)
                    .FirstOrDefaultAsync(m => m.MembershipID == id);
            return mapper.Map<MemberDto>(member);
        }

        /// <summary>
        /// Retrieves a member by their email address.
        /// </summary>
        /// <param name="email">The member's email.</param>
        /// <returns>The <see cref="MemberDto"/> corresponding to the specified email, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="email"/> is null.</exception>
        public async Task<MemberDto> GetMemberByEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null");
            }
            var member = await context.Members
                .Include(m => m.Membership)
                .ThenInclude(ms => ms.MembershipPlan)
                .ThenInclude(ma => ma.Admin)
                    .FirstOrDefaultAsync(m => m.MemberEmail == email);
            return mapper.Map<MemberDto>(member);
        }

        /// <summary>
        /// Creates a new member in the database.
        /// </summary>
        /// <param name="memberDto">The DTO containing the details for the new member.</param>
        /// <param name="membershipId">The ID of the associated membership.</param>
        /// <returns>The created <see cref="MemberDto"/>.</returns>
        public async Task<MemberDto> CreateMember(MemberCreateDto memberDto, Guid membershipId)
        {
            var member = mapper.Map<Member>(memberDto);
            member.MemberId= Guid.NewGuid();
            member.MembershipID = membershipId;
            member.MemberHashedPassword = PasswordHasher.HashPassword(memberDto.MemberHashedPassword);
            var membership = await context.Memberships.FindAsync(membershipId);
            if (membership != null)
            {
                member.Membership = membership;
                var admin = await context.Admins.FindAsync(member.Membership.MembershipPlan.AdminID);
                if(admin != null)
                member.Membership.MembershipPlan.Admin = admin;
            }
            context.Members.Add(member);
            await context.SaveChangesAsync();
            return mapper.Map<MemberDto>(member);
        }

        /// <summary>
        /// Updates an existing member in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the member.</param>
        /// <param name="memberDto">The member update data transfer object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the ID is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the member is not found.</exception>
        public async Task UpdateMember(Guid? id, MemberUpdateDto memberDto)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }
            var member = await context.Members.FindAsync(id);
            if (member == null)
            {
                throw new ArgumentException("Member not found");
            }

            mapper.Map(memberDto, member);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a member from the database.
        /// </summary>
        /// <param name="id">The ID of the member to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown if the ID is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the member is not found.</exception>
        public async Task DeleteMember(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }
            var member = await context.Members.FindAsync(id);
            if (member == null)
            {
                throw new ArgumentException("Member not found");
            }
            var membership = await context.Memberships.FindAsync(member.MembershipID);
            context.Members.Remove(member);
            if(membership != null)
            context.Memberships.Remove(membership);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Changes the password of a member.
        /// </summary>
        /// <param name="id">The ID of the member whose password is to be changed.</param>
        /// <param name="passwordUpdateDto">The DTO containing the current and new passwords.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="id"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the member is not found or the current password is incorrect.</exception>
        public async Task ChangeMemberPassword(Guid? id, PasswordUpdateDto passwordUpdateDto)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }
            var member = await context.Members.FindAsync(id);
            if (member == null)
            {
                throw new ArgumentException("Member not found");
            }
            if (!PasswordHasher.VerifyPassword(passwordUpdateDto.CurrentPassword, member.MemberHashedPassword))
            {
                throw new ArgumentException("Current password is wrong");
            }
            member.MemberHashedPassword = PasswordHasher.HashPassword(passwordUpdateDto.NewPassword);
            await context.SaveChangesAsync();
        }
    }
}
