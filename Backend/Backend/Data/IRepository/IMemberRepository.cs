using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    public interface IMemberRepository
    {
        Task<IEnumerable<MemberDto>> GetAllMembers();
        Task<MemberDto> GetMemberById(Guid? memberId);
        Task<MemberDto> GetMemberByMembershipId(Guid? id);
        Task<MemberDto> GetMemberByEmail(string email);
        Task<MemberDto> CreateMember(MemberCreateDto memberCreateDto, Guid membershipId);
        Task UpdateMember(Guid? memberId, MemberUpdateDto memberUpdateDto);
        Task DeleteMember(Guid? memberId);
        Task ChangeMemberPassword(Guid? memberId, PasswordUpdateDto passwordUpdateDto);
    }
}
