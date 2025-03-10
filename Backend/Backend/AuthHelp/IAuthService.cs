using Backend.Dto.BasicDtos;
using Backend.Entities;

namespace Backend.AuthHelp
{
    public interface IAuthService
    {
        string GenerateToken(string email, string role, Guid id);
        Task<Member> IsMember(LoginDto loginDto);

        Task<Admin> IsAdmin(LoginDto loginDto);

    }
}
