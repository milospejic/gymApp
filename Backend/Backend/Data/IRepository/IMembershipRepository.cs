using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    public interface IMembershipRepository
    {
        Task<IEnumerable<MembershipDto>> GetAllMemberships();
        Task<MembershipDto> GetMembershipById(Guid userId);
        Task<MembershipDto> CreateMembership(MembershipCreateDto userDto);
        Task UpdateMembership(Guid userId, MembershipUpdateDto userDto);
        Task DeleteMembership(Guid userId);
    }
}
