using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;

namespace Backend.Data.IRepository
{
    public interface IMembershipPlanRepository
    {
        Task<IEnumerable<MembershipPlanDto>> GetAllMembershipPlans();
        Task<MembershipPlanDto> GetMembershipPlanById(Guid membershipPlanId);
        Task<MembershipPlanDto> CreateMembershipPlan(MembershipPlanCreateDto membershipPlanCreateDto, Guid? adminId);
        Task UpdateMembershipPlan(Guid membershipPlanId, MembershipPlanUpdateDto membershipPlanUpdateDto, Guid? adminId);
        Task DeleteMembershipPlan(Guid membershipPlanId);
        Task SetPlanForDeletion(Guid id);
    }
}
