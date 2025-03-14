using AutoMapper;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;

namespace Backend.Profiles
{
    /// <summary>
    /// AutoMapper profile for mapping between <see cref="Membership"/> entity and corresponding DTOs.
    /// </summary>
    public class MembershipProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings between <see cref="Membership"/> and its DTOs.
        /// </summary>
        public MembershipProfile()
        {
            // Mapping from Membership entity to MembershipDto
            CreateMap<Membership, MembershipDto>()
                // Mapping MembershipPlan to MembershipPlan in the DTO (this assumes MembershipPlan is a navigation property)
                .ForMember(dest => dest.MembershipPlan, opt => opt.MapFrom(src => src.MembershipPlan));

            // Mapping from MembershipDto to Membership entity
            CreateMap<MembershipDto, Membership>();

            // Mapping from MembershipCreateDto to Membership entity for creation operations
            CreateMap<MembershipCreateDto, Membership>();

            // Mapping from MembershipUpdateDto to Membership entity for update operations
            CreateMap<MembershipUpdateDto, Membership>();
        }
    }
}
