using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.CreateDtos
{
    public class MembershipPlanCreateDto
    {
        [Required(ErrorMessage = "Plan name is required.")]
        [StringLength(100, ErrorMessage = "Plan name can't be longer than 100 characters.")]
        public string PlanName { get; set; }

        [Required(ErrorMessage = "Plan description is required.")]
        [StringLength(500, ErrorMessage = "Plan description can't be longer than 500 characters.")]
        public string PlanDescription { get; set; }

        [Required(ErrorMessage = "Plan price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Plan price must be greater than 0.")]
        public double PlanPrice { get; set; }

    }
}
