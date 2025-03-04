using Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    public class Membership
    {
        [Key]
        public Guid MembershipID { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime MembershipFrom { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime MembershipTo { get; set; }

        [Required(ErrorMessage = "Membership status is required")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string MembershipStatus { get; set; }

        [Required(ErrorMessage = "Plan duration is required")]
        [EnumDataType(typeof(Duration), ErrorMessage = "Invalid duration value")]
        public Duration PlanDuration { get; set; }

        [Required(ErrorMessage = "Membership fee is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Fee must be a positive number")]
        public double MembershipFee { get; set; }

        [Required]
        public Boolean IsFeePaid { get; set; }

        [Required(ErrorMessage = "Membership plan ID is required")]
        public Guid MembershipPlanID { get; set; }

        [ForeignKey("MembershipPlanID")]
        public virtual MembershipPlan MembershipPlan { get; set; }

    }
}