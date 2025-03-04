using AutoMapper;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;

namespace Backend.Profiles
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberDto>()
            .ForMember(dest => dest.MembershipId, opt => opt.MapFrom(src => src.MembershipID))
            .ForMember(dest => dest.Membership, opt => opt.MapFrom(src => src.Membership));
            CreateMap<MemberDto, Member>();
            CreateMap<MemberCreateDto, Member>();
            CreateMap<MemberUpdateDto, Member>();
        }
    }
}
