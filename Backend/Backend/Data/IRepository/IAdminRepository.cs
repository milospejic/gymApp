using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    /// <summary>
    /// Defines methods for managing admin-related operations in the repository.
    /// These operations include retrieving, creating, updating, deleting admin accounts, 
    /// and handling admin password changes.
    /// </summary>
    public interface IAdminRepository
    {
        /// <summary>
        /// Retrieves all administrators.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (starts at 1).</param>
        /// <param name="pageSize">The maximum number of records per page.</param>
        /// <returns>A collection of <see cref="AdminDto"/> representing the admins.</returns>
        Task<IEnumerable<AdminDto>> GetAllAdmins(int pageNumber = 1, int pageSize = 50);

        /// <summary>
        /// Retrieves an admin by their unique identifier.
        /// </summary>
        /// <param name="adminId">The unique identifier of the admin.</param>
        /// <returns>The <see cref="AdminDto"/> of the admin if found; otherwise, null.</returns>
        Task<AdminDto> GetAdminById(Guid? adminId);

        /// <summary>
        /// Retrieves an admin by their email address.
        /// </summary>
        /// <param name="email">The email address of the admin.</param>
        /// <returns>The <see cref="AdminDto"/> of the admin if found; otherwise, null.</returns>
        Task<AdminDto> GetAdminByEmail(string email);

        /// <summary>
        /// Creates a new admin account.
        /// </summary>
        /// <param name="adminDto">The DTO containing the admin details.</param>
        /// <returns>The <see cref="AdminDto"/> of the newly created admin.</returns>
        Task<AdminDto> CreateAdmin(AdminCreateDto adminDto);

        /// <summary>
        /// Updates an existing admin account.
        /// </summary>
        /// <param name="adminId">The unique identifier of the admin to update.</param>
        /// <param name="adminDto">The DTO containing the updated admin details.</param>
        Task UpdateAdmin(Guid? adminId, AdminUpdateDto adminDto);

        /// <summary>
        /// Deletes an admin account.
        /// </summary>
        /// <param name="adminId">The unique identifier of the admin to delete.</param>
        Task DeleteAdmin(Guid? adminId);

        /// <summary>
        /// Changes an admin's password.
        /// </summary>
        /// <param name="adminId">The unique identifier of the admin whose password is being changed.</param>
        /// <param name="passwordUpdateDto">The DTO containing the new password details.</param>
        Task ChangeAdminPassword(Guid? adminId, PasswordUpdateDto passwordUpdateDto);

    }
}
