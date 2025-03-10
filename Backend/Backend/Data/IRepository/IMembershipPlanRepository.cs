using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    public interface IMembershipPlanRepository
    {
        Task<IEnumerable<MembershipPlanDto>> GetAllMembershipPlans();
        Task<MembershipPlanDto> GetMembershipPlanById(Guid membershipPlanId);
        Task<MembershipPlanDto> CreateMembershipPlan(MembershipPlanCreateDto membershipPlanCreateDto);
        Task UpdateMembershipPlan(Guid membershipPlanId, MembershipPlanUpdateDto membershipPlanUpdateDto);
        Task DeleteMembershipPlan(Guid membershipPlanId);
    }
}
