using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    /// <summary>
    /// Defines methods for managing member-related operations in the repository.
    /// These operations include retrieving, creating, updating, deleting member accounts,
    /// and handling member password changes.
    /// </summary>
    public interface IMemberRepository
    {
        
        /// <summary>
        /// Retrieves a paginated list of members.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (starts at 1).</param>
        /// <param name="pageSize">The maximum number of records per page.</param>
        /// <returns>A collection of <see cref="MemberDto"/> representing the members.</returns>
        Task<IEnumerable<MemberDto>> GetAllMembers(int pageNumber = 1, int pageSize = 50);

        /// <summary>
        /// Retrieves a member by their unique identifier.
        /// </summary>
        /// <param name="memberId">The unique identifier of the member.</param>
        /// <returns>The <see cref="MemberDto"/> of the member if found; otherwise, null.</returns>
        Task<MemberDto> GetMemberById(Guid? memberId);

        /// <summary>
        /// Retrieves a member by their membership ID.
        /// </summary>
        /// <param name="id">The unique identifier of the membership.</param>
        /// <returns>The <see cref="MemberDto"/> of the member if found; otherwise, null.</returns>
        Task<MemberDto> GetMemberByMembershipId(Guid? id);

        /// <summary>
        /// Retrieves a member by their email address.
        /// </summary>
        /// <param name="email">The email address of the member.</param>
        /// <returns>The <see cref="MemberDto"/> of the member if found; otherwise, null.</returns>
        Task<MemberDto> GetMemberByEmail(string email);

        /// <summary>
        /// Creates a new member account with the specified membership ID.
        /// </summary>
        /// <param name="memberCreateDto">The DTO containing member details.</param>
        /// <param name="membershipId">The unique identifier of the membership associated with the member.</param>
        /// <returns>The <see cref="MemberDto"/> of the newly created member.</returns>
        Task<MemberDto> CreateMember(MemberCreateDto memberCreateDto, Guid membershipId);

        /// <summary>
        /// Updates an existing member account.
        /// </summary>
        /// <param name="memberId">The unique identifier of the member to update.</param>
        /// <param name="memberUpdateDto">The DTO containing the updated member details.</param>
        Task UpdateMember(Guid? memberId, MemberUpdateDto memberUpdateDto);

        /// <summary>
        /// Deletes a member account.
        /// </summary>
        /// <param name="memberId">The unique identifier of the member to delete.</param>
        Task DeleteMember(Guid? memberId);

        /// <summary>
        /// Changes a member's password.
        /// </summary>
        /// <param name="memberId">The unique identifier of the member whose password is being changed.</param>
        /// <param name="passwordUpdateDto">The DTO containing the new password details.</param>
        Task ChangeMemberPassword(Guid? memberId, PasswordUpdateDto passwordUpdateDto);
    }
}
