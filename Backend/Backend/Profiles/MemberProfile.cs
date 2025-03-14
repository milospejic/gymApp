using AutoMapper;
using Backend.Dto.BasicDtos;
using Backend.Dto.CreateDtos;
using Backend.Dto.UpdateDtos;
using Backend.Entities;

namespace Backend.Profiles
{
    /// <summary>
    /// AutoMapper profile for mapping between <see cref="Member"/> entity and corresponding DTOs.
    /// </summary>
    public class MemberProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings between <see cref="Member"/> and its DTOs.
        /// </summary>
        public MemberProfile()
        {
            // Mapping from Member entity to MemberDto
            CreateMap<Member, MemberDto>()
                // Mapping MembershipID to MembershipId in the DTO
                .ForMember(dest => dest.MembershipId, opt => opt.MapFrom(src => src.MembershipID))
                // Mapping Membership object to Membership property in the DTO
                .ForMember(dest => dest.Membership, opt => opt.MapFrom(src => src.Membership));

            // Mapping from MemberDto to Member entity
            CreateMap<MemberDto, Member>();

            // Mapping from MemberCreateDto to Member entity for creation operations
            CreateMap<MemberCreateDto, Member>();

            // Mapping from MemberUpdateDto to Member entity for update operations
            CreateMap<MemberUpdateDto, Member>();
        }
    }
}
