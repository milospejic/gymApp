using AutoMapper;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Dto.UpdateDtos.Backend.Dto.UpdateDtos;
using Backend.Entities;

namespace Backend.Profiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<Admin, AdminDto>();
            CreateMap<AdminDto, Admin>();
            CreateMap<AdminCreateDto, Admin>();
            CreateMap<AdminUpdateDto, Admin>();
        }
    }
}
