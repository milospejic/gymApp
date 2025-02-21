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
    public class MembershipRepository : IMembershipRepository
    {
        private readonly MyDbContext context;
        private readonly IMapper mapper;

        public MembershipRepository(MyDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<MembershipDto>> GetAllMemberships()
        {
            var memberships = await context.Memberships.ToListAsync();
            return mapper.Map<IEnumerable<MembershipDto>>(memberships);
        }

        public async Task<MembershipDto> GetMembershipById(Guid id)
        {
            var membership = await context.Memberships.FindAsync(id);
            return mapper.Map<MembershipDto>(membership);
        }

        public async Task<MembershipDto> CreateMembership(MembershipCreateDto membershipDto)
        {
            var membership = mapper.Map<Membership>(membershipDto);
            context.Memberships.Add(membership);
            await context.SaveChangesAsync();
            return mapper.Map<MembershipDto>(membership);
        }

        public async Task UpdateMembership(Guid id, MembershipUpdateDto membershipDto)
        {
            var membership = await context.Memberships.FindAsync(id);
            if (membership == null)
            {
                throw new ArgumentException("Membership not found");
            }

            mapper.Map(membershipDto, membership);
            await context.SaveChangesAsync();
        }

        public async Task DeleteMembership(Guid id)
        {
            var membership = await context.Memberships.FindAsync(id);
            if (membership == null)
            {
                throw new ArgumentException("Membership not found");
            }

            context.Memberships.Remove(membership);
            await context.SaveChangesAsync();
        }
    }
}
