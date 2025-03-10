﻿using Backend.Data.Context;
using Backend.Data.IRepository;
using Backend.Data.Repository;
using Backend.Dto.BasicDtos;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.AuthHelp
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly MyDbContext context;

        public AuthService(IConfiguration configuration, MyDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
         
        }

        public async Task<bool> IsMember(LoginDto loginDto)
        {
            var member = await context.Members.SingleOrDefaultAsync(m => m.MemberEmail == loginDto.Email);
            if (member != null && PasswordHasher.VerifyPassword(loginDto.Password, member.MemberHashedPassword))
                return true;
            return false;
        }

        public async Task<bool> IsAdmin(LoginDto loginDto)
        {
            var admin = await context.Admins.SingleOrDefaultAsync(a => a.AdminEmail == loginDto.Email);
            if (admin != null && PasswordHasher.VerifyPassword(loginDto.Password, admin.AdminHashedPassword))
                return true;
            return false;
        }
        
        public string GenerateToken(string email, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
