using AutoMapper;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;

namespace Backend.Profiles
{
    public class MembershipProfile : Profile
    {
        public MembershipProfile()
        {
            CreateMap<Membership, MembershipDto>()
            .ForMember(dest => dest.MembershipPlan, opt => opt.MapFrom(src => src.MembershipPlan));
            CreateMap<MembershipDto, Membership>();
            CreateMap<MembershipCreateDto, Membership>();
            CreateMap<MembershipUpdateDto, Membership>();
        }
    }
}
