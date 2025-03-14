using AutoMapper;
using Backend.Data.Context;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repository
{
    /// <summary>
    /// Implements the <see cref="IAdminRepository"/> interface for managing admin-related operations.
    /// </summary>
    public class AdminRepository : IAdminRepository
    {
        private readonly MyDbContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for querying and saving admin data.</param>
        /// <param name="mapper">The AutoMapper instance used for mapping entities to DTOs.</param>
        public AdminRepository(MyDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Retrieves all admins from the database.
        /// </summary>
        /// <returns>A collection of <see cref="AdminDto"/> representing all admins.</returns>
        public async Task<IEnumerable<AdminDto>> GetAllAdmins()
        {
            var admins = await context.Admins.ToListAsync();
            return mapper.Map<IEnumerable<AdminDto>>(admins);
        }

        /// <summary>
        /// Retrieves an admin by their unique identifier.
        /// </summary>
        /// <param name="id">The admin's ID.</param>
        /// <returns>The <see cref="AdminDto"/> corresponding to the specified ID, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="id"/> is null.</exception>
        public async Task<AdminDto> GetAdminById(Guid? id)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }
            var admin = await context.Admins.FindAsync(id);
            return mapper.Map<AdminDto>(admin);
        }

        /// <summary>
        /// Retrieves an admin by their email address.
        /// </summary>
        /// <param name="email">The admin's email.</param>
        /// <returns>The <see cref="AdminDto"/> corresponding to the specified email, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="email"/> is null.</exception>
        public async Task<AdminDto> GetAdminByEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null");
            }
            var admin = await context.Admins.FirstOrDefaultAsync(a => a.AdminEmail == email);
            return mapper.Map<AdminDto>(admin);
        }

        /// <summary>
        /// Creates a new admin in the database.
        /// </summary>
        /// <param name="adminDto">The DTO containing the details for the new admin.</param>
        /// <returns>The created <see cref="AdminDto"/>.</returns>
        public async Task<AdminDto> CreateAdmin(AdminCreateDto adminDto)
        {
            var admin = mapper.Map<Admin>(adminDto);
            admin.AdminId = Guid.NewGuid();
            admin.AdminHashedPassword = PasswordHasher.HashPassword(adminDto.AdminHashedPassword);
            context.Admins.Add(admin);
            await context.SaveChangesAsync();
            return mapper.Map<AdminDto>(admin);
        }

        /// <summary>
        /// Updates the details of an existing admin.
        /// </summary>
        /// <param name="id">The ID of the admin to update.</param>
        /// <param name="adminDto">The DTO containing the updated details for the admin.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="id"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the admin is not found.</exception>
        public async Task UpdateAdmin(Guid? id, AdminUpdateDto adminDto)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }
            var admin = await context.Admins.FindAsync(id);
            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

            mapper.Map(adminDto, admin);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an admin from the database.
        /// </summary>
        /// <param name="id">The ID of the admin to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="id"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the admin is not found.</exception>
        public async Task DeleteAdmin(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }
            var admin = await context.Admins.FindAsync(id);
            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

            context.Admins.Remove(admin);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Changes the password of an admin.
        /// </summary>
        /// <param name="id">The ID of the admin whose password is to be changed.</param>
        /// <param name="passwordUpdateDto">The DTO containing the current and new passwords.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="id"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the admin is not found or the current password is incorrect.</exception>
        public async Task ChangeAdminPassword(Guid? id, PasswordUpdateDto passwordUpdateDto)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }

            var admin = await context.Admins.FindAsync(id);
            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

        
            if (!PasswordHasher.VerifyPassword(passwordUpdateDto.CurrentPassword, admin.AdminHashedPassword))
            {
                throw new ArgumentException("Current password is wrong");
            }

            admin.AdminHashedPassword = PasswordHasher.HashPassword(passwordUpdateDto.NewPassword);
            await context.SaveChangesAsync();
        }

    }


}
