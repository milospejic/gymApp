using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    /// <summary>
    /// Defines methods for managing membership plan-related operations in the repository.
    /// These operations include retrieving, creating, updating, deleting membership plans, 
    /// and setting a plan for deletion.
    /// </summary>
    public interface IMembershipPlanRepository
    {
        /// <summary>
        /// Retrieves all available membership plans.
        /// </summary>
        /// <returns>A collection of <see cref="MembershipPlanDto"/> representing all membership plans.</returns>
        Task<IEnumerable<MembershipPlanDto>> GetAllMembershipPlans();

        /// <summary>
        /// Retrieves a membership plan by its unique identifier.
        /// </summary>
        /// <param name="membershipPlanId">The unique identifier of the membership plan.</param>
        /// <returns>The <see cref="MembershipPlanDto"/> if found; otherwise, null.</returns>
        Task<MembershipPlanDto> GetMembershipPlanById(Guid membershipPlanId);

        /// <summary>
        /// Creates a new membership plan.
        /// </summary>
        /// <param name="membershipPlanCreateDto">The DTO containing the membership plan details.</param>
        /// <param name="adminId">The unique identifier of the admin creating the plan.</param>
        /// <returns>The <see cref="MembershipPlanDto"/> of the newly created membership plan.</returns>
        Task<MembershipPlanDto> CreateMembershipPlan(MembershipPlanCreateDto membershipPlanCreateDto, Guid? adminId);

        /// <summary>
        /// Updates an existing membership plan.
        /// </summary>
        /// <param name="membershipPlanId">The unique identifier of the membership plan to update.</param>
        /// <param name="membershipPlanUpdateDto">The DTO containing updated membership plan details.</param>
        /// <param name="adminId">The unique identifier of the admin updating the plan.</param>
        Task UpdateMembershipPlan(Guid membershipPlanId, MembershipPlanUpdateDto membershipPlanUpdateDto, Guid? adminId);

        /// <summary>
        /// Deletes a membership plan.
        /// </summary>
        /// <param name="membershipPlanId">The unique identifier of the membership plan to delete.</param>
        Task DeleteMembershipPlan(Guid membershipPlanId);

        /// <summary>
        /// Changes a membership plan ForDeletion property.
        /// </summary>
        /// <param name="id">The unique identifier of the membership plan to mark for deletion.</param>
        Task SetPlanForDeletion(Guid id);
    }
}
