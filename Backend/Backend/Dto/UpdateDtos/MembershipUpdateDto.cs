using Backend.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.UpdateDtos
{
    public class MembershipUpdateDto
    {
        [Required(ErrorMessage = "Membership plan duration is required.")]
        [EnumDataType(typeof(Duration), ErrorMessage = "Invalid duration value.")]
        public Duration PlanDuration { get; set; }

        [Required(ErrorMessage = "Membership plan ID is required.")]
        public Guid MembershipPlanID { get; set; }
    }
}
