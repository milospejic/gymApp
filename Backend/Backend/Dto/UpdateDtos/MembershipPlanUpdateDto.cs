using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.UpdateDtos
{
    /// <summary>
    /// Data Transfer Object for creating a new membership plan.
    /// </summary>
    public class MembershipPlanUpdateDto
    {
        /// <summary>
        /// Name of the membership plan.
        /// </summary>
        [Required(ErrorMessage = "Plan name is required.")]
        [StringLength(100, ErrorMessage = "Plan name can't be longer than 100 characters.")]
        public string PlanName { get; set; }

        /// <summary>
        /// Description of the membership plan.
        /// </summary>
        [StringLength(500, ErrorMessage = "Plan description can't be longer than 500 characters.")]
        public string PlanDescription { get; set; }

        /// <summary>
        /// Price of the membership plan.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Plan price must be greater than 0.")]
        public double PlanPrice { get; set; }

    }
}
