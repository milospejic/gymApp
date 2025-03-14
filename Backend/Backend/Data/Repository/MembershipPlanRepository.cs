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
    /// <summary>
    /// Implements the <see cref="IMembershipPlanRepository"/> interface for managing membership plan-related operations.
    /// </summary>
    public class MembershipPlanRepository : IMembershipPlanRepository
    {
        private readonly MyDbContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipPlanRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for querying and saving membership plan data.</param>
        /// <param name="mapper">The AutoMapper instance used for mapping entities to DTOs.</param>
        public MembershipPlanRepository(MyDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Retrieves all membership plans from the database.
        /// </summary>
        /// <returns>A collection of <see cref="MembershipPlanDto"/> representing all membership plans.</returns>
        public async Task<IEnumerable<MembershipPlanDto>> GetAllMembershipPlans()
        {
            var membershipPlans = await context.MembershipPlans
                 .Include(m => m.Admin)
                 .ToListAsync();
            return mapper.Map<IEnumerable<MembershipPlanDto>>(membershipPlans);
        }

        /// <summary>
        /// Retrieves a membership plan by its unique ID.
        /// </summary>
        /// <param name="id">The memberships's ID.</param>
        /// <returns>The corresponding <see cref="MembershipPlanDto"/>.</returns>
        public async Task<MembershipPlanDto> GetMembershipPlanById(Guid id)
        {
            var membershipPlan = await context.MembershipPlans
                .Include(m => m.Admin)
                .FirstOrDefaultAsync(mp => mp.PlanID == id);
            return mapper.Map<MembershipPlanDto>(membershipPlan);
        }

        /// <summary>
        /// Creates a new membership plan.
        /// </summary>
        /// <param name="membershipPlanDto">The DTO containing the details for the new membership plan.</param>
        /// <param name="adminId">The unique identifier of the admin responsible for the plan.</param>
        /// <returns>The created <see cref="MembershipPlanDto"/>.</returns>
        public async Task<MembershipPlanDto> CreateMembershipPlan(MembershipPlanCreateDto membershipPlanDto, Guid? adminId)
        {
            var membershipPlan = mapper.Map<MembershipPlan>(membershipPlanDto);
            membershipPlan.PlanID = Guid.NewGuid();
            membershipPlan.AdminID = adminId;
            var admin =await context.Admins.FindAsync(adminId);
            if(admin != null)
            membershipPlan.Admin = admin;
            context.MembershipPlans.Add(membershipPlan);
            await context.SaveChangesAsync();
            return mapper.Map<MembershipPlanDto>(membershipPlan);
        }

        /// <summary>
        /// Updates the details of an membership plan.
        /// </summary>
        /// <param name="id">The ID of the membership plan to update.</param>
        /// <param name="membershipPlanDto">The DTO containing updated details for the membership plan.</param>
        /// <param name="adminId">The unique identifier of the admin responsible for the plan.</param>
        /// <exception cref="ArgumentException">Thrown if the membership plan is not found.</exception>
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

        /// <summary>
        /// Deletes a membership plan if it is marked for deletion and has no active memberships.
        /// </summary>
        /// <param name="id">The ID of the membership plan to delete.</param>
        /// <exception cref="ArgumentException">Thrown if the membership plan is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the plan is not marked for deletion or has active memberships.</exception>
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

        /// <summary>
        /// Toggles the deletion status of a membership plan.
        /// </summary>
        /// <param name="id">The unique identifier of the membership plan to update.</param>
        /// <exception cref="ArgumentException">Thrown if the membership plan is not found.</exception>
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
