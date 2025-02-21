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
    public class MembershipPlanRepository : IMembershipPlanRepository
    {
        private readonly MyDbContext context;
        private readonly IMapper mapper;

        public MembershipPlanRepository(MyDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<MembershipPlanDto>> GetAllMembershipPlans()
        {
            var membershipPlans = await context.MembershipPlans.ToListAsync();
            return mapper.Map<IEnumerable<MembershipPlanDto>>(membershipPlans);
        }

        public async Task<MembershipPlanDto> GetMembershipPlanById(Guid id)
        {
            var membershipPlan = await context.MembershipPlans.FindAsync(id);
            return mapper.Map<MembershipPlanDto>(membershipPlan);
        }

        public async Task<MembershipPlanDto> CreateMembershipPlan(MembershipPlanCreateDto membershipPlanDto)
        {
            var membershipPlan = mapper.Map<MembershipPlan>(membershipPlanDto);
            context.MembershipPlans.Add(membershipPlan);
            await context.SaveChangesAsync();
            return mapper.Map<MembershipPlanDto>(membershipPlan);
        }

        public async Task UpdateMembershipPlan(Guid id, MembershipPlanUpdateDto membershipPlanDto)
        {
            var membershipPlan = await context.MembershipPlans.FindAsync(id);
            if (membershipPlan == null)
            {
                throw new ArgumentException("MembershipPlan not found");
            }

            mapper.Map(membershipPlanDto, membershipPlan);
            await context.SaveChangesAsync();
        }

        public async Task DeleteMembershipPlan(Guid id)
        {
            var membershipPlan = await context.MembershipPlans.FindAsync(id);
            if (membershipPlan == null)
            {
                throw new ArgumentException("MembershipPlan not found");
            }

            context.MembershipPlans.Remove(membershipPlan);
            await context.SaveChangesAsync();
        }
    }
}
