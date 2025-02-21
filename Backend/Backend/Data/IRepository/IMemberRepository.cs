﻿using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    public interface IMemberRepository
    {
        Task<IEnumerable<MemberDto>> GetAllMembers();
        Task<MemberDto> GetMemberById(Guid userId);
        Task<MemberDto> CreateMember(MemberCreateDto userDto);
        Task UpdateMember(Guid userId, MemberUpdateDto userDto);
        Task DeleteMember(Guid userId);
    }
}
