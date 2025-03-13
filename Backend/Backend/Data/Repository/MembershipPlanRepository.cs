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
            var membershipPlans = await context.MembershipPlans
                 .Include(m => m.Admin)
                 .ToListAsync();
            return mapper.Map<IEnumerable<MembershipPlanDto>>(membershipPlans);
        }

        public async Task<MembershipPlanDto> GetMembershipPlanById(Guid id)
        {
            var membershipPlan = await context.MembershipPlans
                .Include(m => m.Admin)
                .FirstOrDefaultAsync(mp => mp.PlanID == id);
            return mapper.Map<MembershipPlanDto>(membershipPlan);
        }

        public async Task<MembershipPlanDto> CreateMembershipPlan(MembershipPlanCreateDto membershipPlanDto, Guid? adminId)
        {
            var membershipPlan = mapper.Map<MembershipPlan>(membershipPlanDto);
            membershipPlan.PlanID = Guid.NewGuid();
            membershipPlan.AdminID = adminId;
            membershipPlan.Admin = await context.Admins.FindAsync(adminId);
            context.MembershipPlans.Add(membershipPlan);
            await context.SaveChangesAsync();
            return mapper.Map<MembershipPlanDto>(membershipPlan);
        }

        public async Task UpdateMembershipPlan(Guid id, MembershipPlanUpdateDto membershipPlanDto, Guid? adminId)
        {
            var membershipPlan = await context.MembershipPlans.FindAsync(id);
            if (membershipPlan == null)
            {
                throw new ArgumentException("MembershipPlan not found");
            }

            mapper.Map(membershipPlanDto, membershipPlan);
            membershipPlan.AdminID = adminId;
            await context.SaveChangesAsync();
        }

        public async Task DeleteMembershipPlan(Guid id)
        {
            var membershipPlan = await context.MembershipPlans
                .Include(mp => mp.Memberships)
                .FirstOrDefaultAsync(mp => mp.PlanID == id);

            if (membershipPlan == null)
            {
                throw new ArgumentException("Membership plan not found");
            }

            if (!membershipPlan.ForDeletion)
            {
                throw new InvalidOperationException("Plan is not set for deletion");
            }

            if (membershipPlan.Memberships.Any(m => m.MembershipTo >= DateTime.Now))
            {
                throw new InvalidOperationException("There are still active memberships on this plan");
            }

            context.MembershipPlans.Remove(membershipPlan);
            await context.SaveChangesAsync();
        }


        public async Task SetPlanForDeletion(Guid id)
        {
            var membershipPlan = await context.MembershipPlans.FindAsync(id);
            if (membershipPlan == null)
            {
                throw new ArgumentException("MembershipPlan not found");
            }

            membershipPlan.ForDeletion = !membershipPlan.ForDeletion;
            await context.SaveChangesAsync();
        }
    }
}
