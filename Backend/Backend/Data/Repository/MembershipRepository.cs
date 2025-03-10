using AutoMapper;
using Backend.Data.Context;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;
using Backend.Enums;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Numerics;

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
            var memberships = await context.Memberships
                 .Include(m => m.MembershipPlan)
                 .ThenInclude(ms => ms.Admin)
                 .ToListAsync();
            return mapper.Map<IEnumerable<MembershipDto>>(memberships);
        }

        public async Task<MembershipDto> GetMembershipById(Guid id)
        {
            var membership = await context.Memberships
                .Include(m => m.MembershipPlan)
                .ThenInclude(ma => ma.Admin)
                .FirstOrDefaultAsync(ms => ms.MembershipID == id);
            return mapper.Map<MembershipDto>(membership);
        }

        public async Task<MembershipDto> CreateMembership(MembershipCreateDto membershipDto)
        {

            var plan = await context.MembershipPlans.FindAsync(membershipDto.MembershipPlanID);
            if (plan == null)
            {
                throw new ArgumentException("Plan was not found");
            }
             
            var membership = mapper.Map<Membership>(membershipDto);
            membership.MembershipFrom = DateTime.Now;
            membership.MembershipStatus = "Active";
            switch (membership.PlanDuration)
            {
                case Duration.OneMonth:
                    membership.MembershipTo = DateTime.Now.AddMonths(1);
                    membership.MembershipFee = plan.PlanPrice;
                    break;
                case Duration.ThreeMonths:
                    membership.MembershipTo = DateTime.Now.AddMonths(3);
                    membership.MembershipFee = plan.PlanPrice * 3 * 0.9;
                    break;
                case Duration.SixMonths:
                    membership.MembershipTo = DateTime.Now.AddMonths(6);
                    membership.MembershipFee = plan.PlanPrice * 6 * 0.8;
                    break;
                case Duration.OneYear:
                    membership.MembershipTo = DateTime.Now.AddMonths(12);
                    membership.MembershipFee = plan.PlanPrice * 12 * 0.7;
                    break;
                default:
                    break;
            }
            membership.IsFeePaid = false;
            
            context.Memberships.Add(membership);
            await context.SaveChangesAsync();
            return mapper.Map<MembershipDto>(membership);
        }

        public async Task UpdateMembership(Guid? id, MembershipUpdateDto membershipDto)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }
            var membership = await context.Memberships.FindAsync(id);
            if (membership == null)
            {
                throw new ArgumentException("Membership not found");
            }
            if (membership.MembershipTo > DateTime.Now)
            {
                throw new ArgumentException("You cannot renew membership because you still have an active one.");
            }

            var plan = await context.MembershipPlans.FindAsync(membershipDto.MembershipPlanID);
            if (plan == null) {
                throw new ArgumentException("Plan not found");
                    
            }
            mapper.Map(membershipDto, membership);
            membership.MembershipFrom = DateTime.Now;
            membership.MembershipStatus = "Active";
            switch (membership.PlanDuration)
            {
                case Duration.OneMonth:
                    membership.MembershipTo = DateTime.Now.AddMonths(1);
                    membership.MembershipFee = plan.PlanPrice;
                    break;
                case Duration.ThreeMonths:
                    membership.MembershipTo = DateTime.Now.AddMonths(3);
                    membership.MembershipFee = plan.PlanPrice * 3 * 0.9;
                    break;
                case Duration.SixMonths:
                    membership.MembershipTo = DateTime.Now.AddMonths(6);
                    membership.MembershipFee = plan.PlanPrice * 6 * 0.8;
                    break;
                case Duration.OneYear:
                    membership.MembershipTo = DateTime.Now.AddMonths(12);
                    membership.MembershipFee = plan.PlanPrice * 12 * 0.7;
                    break;
                default:
                    break;
            }
            membership.IsFeePaid = false;
      
            await context.SaveChangesAsync();
        }


    }
}
