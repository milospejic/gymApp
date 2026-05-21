using Backend.Dto.BasicDtos;
using System.Security.Claims;

namespace Backend.AuthHelp
{
    public interface IAuthService
    {
        Task<TokenModelDto?> AuthenticateAsync(LoginDto loginDto);
        Task<TokenModelDto?> RefreshTokensAsync(TokenModelDto tokenModel);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}