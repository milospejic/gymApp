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
            CreateMap<MembershipPlan, MembershipPlanDto>()
            .ForMember(dest => dest.Admin, opt => opt.MapFrom(src => src.Admin));
            CreateMap<MembershipPlanDto, MembershipPlan>();
            CreateMap<MembershipPlanCreateDto, MembershipPlan>();
            CreateMap<MembershipPlanUpdateDto, MembershipPlan>();
        }
    }
}
