using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Dto.UpdateDtos.Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    public interface IAdminRepository
    {
        Task<IEnumerable<AdminDto>> GetAllAdmins();
        Task<AdminDto> GetAdminById(Guid? adminId);
        Task<AdminDto> CreateAdmin(AdminCreateDto adminDto);
        Task UpdateAdmin(Guid? adminId, AdminUpdateDto adminDto);
        Task DeleteAdmin(Guid? adminId);
        Task ChangeAdminPassword(Guid? adminId, PasswordUpdateDto passwordUpdateDto);

    }
}
