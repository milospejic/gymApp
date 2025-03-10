using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    public interface IMembershipRepository
    {
        Task<IEnumerable<MembershipDto>> GetAllMemberships();
        Task<MembershipDto> GetMembershipById(Guid membershipId);
        Task<MembershipDto> CreateMembership(MembershipCreateDto membershipCreateDto);
        Task UpdateMembership(Guid? membershipId, MembershipUpdateDto membershipUpdateDto);

    }
}
