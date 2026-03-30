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
        /// Retrieves a paginated list of memberships.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (starts at 1).</param>
        /// <param name="pageSize">The maximum number of records per page.</param>
        /// <returns>A collection of <see cref="MembershipDto"/> representing the memberships.</returns>
        Task<IEnumerable<MembershipDto>> GetAllMemberships(int pageNumber = 1, int pageSize = 50);

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
