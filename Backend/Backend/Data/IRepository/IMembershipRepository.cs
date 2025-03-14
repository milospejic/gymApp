using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    /// <summary>
    /// Defines methods for managing membership-related operations in the repository.
    /// These operations include retrieving, creating, and updating membership records.
    /// </summary>
    public interface IMembershipRepository
    {
        /// <summary>
        /// Retrieves all memberships.
        /// </summary>
        /// <returns>A collection of <see cref="MembershipDto"/> representing all memberships.</returns>
        Task<IEnumerable<MembershipDto>> GetAllMemberships();

        /// <summary>
        /// Retrieves a membership by its unique identifier.
        /// </summary>
        /// <param name="membershipId">The unique identifier of the membership.</param>
        /// <returns>The <see cref="MembershipDto"/> if found; otherwise, null.</returns>
        Task<MembershipDto> GetMembershipById(Guid membershipId);

        /// <summary>
        /// Creates a new membership.
        /// </summary>
        /// <param name="membershipCreateDto">The DTO containing the new membership details.</param>
        /// <returns>The <see cref="MembershipDto"/> of the newly created membership.</returns>
        Task<MembershipDto> CreateMembership(MembershipCreateDto membershipCreateDto);

        /// <summary>
        /// Updates an existing membership.
        /// </summary>
        /// <param name="membershipId">The unique identifier of the membership to update.</param>
        /// <param name="membershipUpdateDto">The DTO containing updated membership details.</param>
        Task UpdateMembership(Guid? membershipId, MembershipUpdateDto membershipUpdateDto);
    }
}
