using AutoMapper;
using Backend.Data.Context;
using Backend.Data.IRepository;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Dto.UpdateDtos.Backend.Dto.UpdateDtos;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MyDbContext context;
        private readonly IMapper mapper;

        public AdminRepository(MyDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<AdminDto>> GetAllAdmins()
        {
            var admins = await context.Admins.ToListAsync();
            return mapper.Map<IEnumerable<AdminDto>>(admins);
        }

        public async Task<AdminDto> GetAdminById(Guid id)
        {
            var admin = await context.Admins.FindAsync(id);
            return mapper.Map<AdminDto>(admin);
        }

        public async Task<AdminDto> CreateAdmin(AdminCreateDto adminDto)
        {
            var admin = mapper.Map<Admin>(adminDto);
            admin.AdminId = Guid.NewGuid();
            context.Admins.Add(admin);
            await context.SaveChangesAsync();
            return mapper.Map<AdminDto>(admin);
        }

        public async Task UpdateAdmin(Guid id, AdminUpdateDto adminDto)
        {
            var admin = await context.Admins.FindAsync(id);
            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

            mapper.Map(adminDto, admin);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAdmin(Guid id)
        {
            var admin = await context.Admins.FindAsync(id);
            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }

            context.Admins.Remove(admin);
            await context.SaveChangesAsync();
        }

        public async Task ChangeAdminPassword(Guid id, PasswordUpdateDto passwordUpdateDto)
        {
            var admin = await context.Admins.FindAsync(id);
            if (admin == null)
            {
                throw new ArgumentException("Admin not found");
            }
            if(admin.AdminHashedPassword != passwordUpdateDto.CurrentPassword)
            {
                throw new ArgumentException("Current password is wrong");
            }
            admin.AdminHashedPassword = passwordUpdateDto.NewPassword;
            await context.SaveChangesAsync();
        }
    }

   
}
