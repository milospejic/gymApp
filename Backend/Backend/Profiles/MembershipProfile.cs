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
            CreateMap<Membership, MembershipDto>();
            CreateMap<MembershipDto, Membership>();
            CreateMap<MembershipCreateDto, Membership>();
            CreateMap<MembershipUpdateDto, Membership>();
        }
    }
}
