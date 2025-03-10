using Backend.Dto.BasicDtos;

namespace Backend.AuthHelp
{
    public interface IAuthService
    {
        string GenerateToken(string email, string role);
        Task<bool> IsMember(LoginDto loginDto);

        Task<bool> IsAdmin(LoginDto loginDto);

    }
}
