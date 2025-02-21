using AutoMapper;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;

namespace Backend.Profiles
{
    public class MembershipPlanProfile : Profile
    {
        public MembershipPlanProfile() 
        {
            CreateMap<MembershipPlan, MembershipPlanDto>();
            CreateMap<MembershipPlanDto, MembershipPlan>();
            CreateMap<MembershipPlanCreateDto, MembershipPlan>();
            CreateMap<MembershipPlanUpdateDto, MembershipPlan>();
        }
    }
}
