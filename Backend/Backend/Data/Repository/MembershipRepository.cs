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
    /// <summary>
    /// Implements the <see cref="IMembershipRepository"/> interface for managing membership-related operations.
    /// </summary>
    public class MembershipRepository : IMembershipRepository
    {
        private readonly MyDbContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for querying and saving membership data.</param>
        /// <param name="mapper">The AutoMapper instance used for mapping entities to DTOs.</param>
        public MembershipRepository(MyDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Retrieves all memberships from the database.
        /// </summary>
        /// <returns>A collection of <see cref="MembershipDto"/> representing all memberships.</returns>
        public async Task<IEnumerable<MembershipDto>> GetAllMemberships()
        {
            var memberships = await context.Memberships
                 .Include(m => m.MembershipPlan)
                 .ThenInclude(ms => ms.Admin)
                 .ToListAsync();
            return mapper.Map<IEnumerable<MembershipDto>>(memberships);
        }

        /// <summary>
        /// Retrieves an membership by their unique identifier.
        /// </summary>
        /// <param name="id">The memberships's ID.</param>
        /// <returns>The <see cref="MembershipDto"/> corresponding to the specified ID, or null if not found.</returns>
        public async Task<MembershipDto> GetMembershipById(Guid id)
        {
           
            var membership = await context.Memberships
                .Include(m => m.MembershipPlan)
                .ThenInclude(ma => ma.Admin)
                .FirstOrDefaultAsync(ms => ms.MembershipID == id);
            return mapper.Map<MembershipDto>(membership);
        }

        /// <summary>
        /// Creates a new membership in the database.
        /// </summary>
        /// <param name="membershipDto">The DTO containing the details for the new membership.</param>
        /// <returns>The created <see cref="MembershipDto"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified membership plan is not found.</exception>
        public async Task<MembershipDto> CreateMembership(MembershipCreateDto membershipDto)
        {

            var plan = await context.MembershipPlans.FindAsync(membershipDto.MembershipPlanID);
            if (plan == null)
            {
                throw new ArgumentException("Plan was not found");
            }
             
            var membership = mapper.Map<Membership>(membershipDto);
            membership.MembershipFrom = DateTime.Now;
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

        /// <summary>
        /// Updates the details of an existing membership.
        /// </summary>
        /// <param name="id">The ID of the membership to update.</param>
        /// <param name="membershipDto">The DTO containing the updated details for the membership.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided ID is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the membership or plan is not found, or when attempting to renew an active membership.</exception>
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
