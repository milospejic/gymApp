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
    public class MemberRepository : IMemberRepository
    {
        private readonly MyDbContext context;
        private readonly IMapper mapper;

        public MemberRepository(MyDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<MemberDto>> GetAllMembers()
        {
            var members = await context.Members
                .Include(m => m.Membership)
                    .ThenInclude(ms => ms.MembershipPlan)
                    .ThenInclude(ma => ma.Admin)
                    .ToListAsync();
            return mapper.Map<IEnumerable<MemberDto>>(members);
        }

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

        public async Task<MemberDto> CreateMember(MemberCreateDto memberDto, Guid membershipId)
        {
            var member = mapper.Map<Member>(memberDto);
            member.MemberId= Guid.NewGuid();
            member.MembershipID = membershipId;
            member.MemberHashedPassword = PasswordHasher.HashPassword(memberDto.MemberHashedPassword);
            member.Membership = await context.Memberships.FindAsync(membershipId);
            member.Membership.MembershipPlan.Admin = await context.Admins.FindAsync(member.Membership.MembershipPlan.AdminID);
            context.Members.Add(member);
            await context.SaveChangesAsync();
            return mapper.Map<MemberDto>(member);
        }

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
            var memberhip = await context.Memberships.FindAsync(member.MembershipID);
            context.Members.Remove(member);
            context.Memberships.Remove(memberhip);
            await context.SaveChangesAsync();
        }

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
