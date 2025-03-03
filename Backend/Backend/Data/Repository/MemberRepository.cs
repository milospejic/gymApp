using AutoMapper;
using Backend.Data.Context;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;
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
            var members = await context.Members.ToListAsync();
            return mapper.Map<IEnumerable<MemberDto>>(members);
        }

        public async Task<MemberDto> GetMemberById(Guid id)
        {
            var member = await context.Members.FindAsync(id);
            return mapper.Map<MemberDto>(member);
        }

        public async Task<MemberDto> CreateMember(MemberCreateDto memberDto, Guid membershipId)
        {
            var member = mapper.Map<Member>(memberDto);
            member.MemberId= Guid.NewGuid();
            member.MembershipID = membershipId;
            context.Members.Add(member);
            await context.SaveChangesAsync();
            return mapper.Map<MemberDto>(member);
        }

        public async Task UpdateMember(Guid id, MemberUpdateDto memberDto)
        {
            var member = await context.Members.FindAsync(id);
            if (member == null)
            {
                throw new ArgumentException("Member not found");
            }

            mapper.Map(memberDto, member);
            await context.SaveChangesAsync();
        }

        public async Task DeleteMember(Guid id)
        {
            var member = await context.Members.FindAsync(id);
            if (member == null)
            {
                throw new ArgumentException("Member not found");
            }
            context.Memberships.Remove(context.Memberships.Find(member.MembershipID));
            context.Members.Remove(member);
            await context.SaveChangesAsync();
        }

        public async Task ChangeMemberPassword(Guid id, PasswordUpdateDto passwordUpdateDto)
        {
            var member = await context.Members.FindAsync(id);
            if (member == null)
            {
                throw new ArgumentException("Member not found");
            }
            if (member.MemberHashedPassword != passwordUpdateDto.CurrentPassword)
            {
                throw new ArgumentException("Current password is wrong");
            }
            member.MemberHashedPassword = passwordUpdateDto.NewPassword;
            await context.SaveChangesAsync();
        }
    }
}
