using Backend.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.UpdateDtos
{
    /// <summary>
    /// Data Transfer Object for updating a Membership entity.
    /// </summary>
    public class MembershipUpdateDto
    {
        /// <summary>
        /// Duration of the membership plan.
        /// </summary>
        [Required(ErrorMessage = "Membership plan duration is required.")]
        [EnumDataType(typeof(Duration), ErrorMessage = "Invalid duration value.")]
        public Duration PlanDuration { get; set; }

        /// <summary>
        /// Unique identifier for the selected membership plan.
        /// </summary>
        [Required(ErrorMessage = "Membership plan ID is required.")]
        public Guid MembershipPlanID { get; set; }
    }
}
