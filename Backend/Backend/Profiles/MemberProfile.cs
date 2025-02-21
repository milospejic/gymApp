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
            CreateMap<Member, MemberDto>();
            CreateMap<MemberDto, Member>();
            CreateMap<MemberCreateDto, Member>();
            CreateMap<MemberUpdateDto, Member>();
        }
    }
}
