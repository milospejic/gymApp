using AutoMapper;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;

namespace Backend.Profiles
{
    /// <summary>
    /// AutoMapper profile for mapping between <see cref="Admin"/> entity and corresponding DTOs.
    /// </summary>
    public class AdminProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings between <see cref="Admin"/> and its DTOs.
        /// </summary>
        public AdminProfile()
        {
            // Mapping from Admin entity to AdminDto
            CreateMap<Admin, AdminDto>();

            // Mapping from AdminDto to Admin entity
            CreateMap<AdminDto, Admin>();

            // Mapping from AdminCreateDto to Admin entity for creation operations
            CreateMap<AdminCreateDto, Admin>();

            // Mapping from AdminUpdateDto to Admin entity for update operations
            CreateMap<AdminUpdateDto, Admin>();
        }
    }
}
