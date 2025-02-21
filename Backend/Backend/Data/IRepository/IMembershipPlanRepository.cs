using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    public interface IMembershipPlanRepository
    {
        Task<IEnumerable<MembershipPlanDto>> GetAllMembershipPlans();
        Task<MembershipPlanDto> GetMembershipPlanById(Guid userId);
        Task<MembershipPlanDto> CreateMembershipPlan(MembershipPlanCreateDto userDto);
        Task UpdateMembershipPlan(Guid userId, MembershipPlanUpdateDto userDto);
        Task DeleteMembershipPlan(Guid userId);
    }
}
