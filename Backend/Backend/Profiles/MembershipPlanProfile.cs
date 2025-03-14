using AutoMapper;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;

namespace Backend.Profiles
{
    /// <summary>
    /// AutoMapper profile for mapping between <see cref="MembershipPlan"/> entity and corresponding DTOs.
    /// </summary>
    public class MembershipPlanProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings between <see cref="MembershipPlan"/> and its DTOs.
        /// </summary>
        public MembershipPlanProfile()
        {
            // Mapping from MembershipPlan entity to MembershipPlanDto
            CreateMap<MembershipPlan, MembershipPlanDto>()
                // Mapping Admin to Admin in the DTO (this assumes Admin is a navigation property)
                .ForMember(dest => dest.Admin, opt => opt.MapFrom(src => src.Admin));

            // Mapping from MembershipPlanDto to MembershipPlan entity
            CreateMap<MembershipPlanDto, MembershipPlan>();

            // Mapping from MembershipPlanCreateDto to MembershipPlan entity for creation operations
            CreateMap<MembershipPlanCreateDto, MembershipPlan>();

            // Mapping from MembershipPlanUpdateDto to MembershipPlan entity for update operations
            CreateMap<MembershipPlanUpdateDto, MembershipPlan>();
        }
    }
}
