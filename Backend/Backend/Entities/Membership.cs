using Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    /// <summary>
    /// Represents a membership entity containing subscription details for a member.
    /// </summary>
    public class Membership
    {
        /// <summary>
        /// Unique identifier for the membership.
        /// </summary>
        [Key]
        public Guid MembershipID { get; set; }

        /// <summary>
        /// Date when the membership starts.
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        public DateTime MembershipFrom { get; set; }

        /// <summary>
        /// Date when the membership ends.
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        public DateTime MembershipTo { get; set; }

        /// <summary>
        /// Duration of the membership plan.
        /// </summary>
        [Required(ErrorMessage = "Plan duration is required")]
        [EnumDataType(typeof(Duration), ErrorMessage = "Invalid duration value")]
        public Duration PlanDuration { get; set; }

        /// <summary>
        /// Membership fee amount.
        /// </summary>
        [Required(ErrorMessage = "Membership fee is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Fee must be a positive number")]
        public double MembershipFee { get; set; }

        /// <summary>
        /// Indicates whether the membership fee has been paid.
        /// </summary>
        [Required]
        public Boolean IsFeePaid { get; set; }

        /// <summary>
        /// Foreign key linking to the Membership Plan.
        /// </summary>
        [Required(ErrorMessage = "Membership plan ID is required")]
        public Guid? MembershipPlanID { get; set; }

        /// <summary>
        /// Navigation property for the associated Membership Plan.
        /// </summary>
        [ForeignKey("MembershipPlanID")]
        public virtual MembershipPlan? MembershipPlan { get; set; }

    }
}