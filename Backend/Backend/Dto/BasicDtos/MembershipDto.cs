using Backend.Enums;

namespace Backend.Dto.BasicDtos
{
    /// <summary>
    /// Data Transfer Object for Membership entity.
    /// </summary>
    public class MembershipDto
    {
        /// <summary>
        /// Unique identifier for the membership.
        /// </summary>
        public Guid MembershipID { get; set; }

        /// <summary>
        /// Start date of the membership.
        /// </summary>
        public DateTime MembershipFrom { get; set; }

        /// <summary>
        /// Expiry date of the membership.
        /// </summary>
        public DateTime MembershipTo { get; set; }

        /// <summary>
        /// Duration of the membership plan.
        /// </summary>
        public Duration PlanDuration { get; set; }

        /// <summary>
        /// Membership fee amount.
        /// </summary>
        public double MembershipFee { get; set; }

        /// <summary>
        /// Indicates if the membership fee has been paid.
        /// </summary>
        public bool IsFeePaid { get; set; }

        /// <summary>
        /// Unique identifier of the associated membership plan.
        /// </summary>
        public Guid? MembershipPlanId { get; set; }

        /// <summary>
        /// Membership plan details associated with this membership.
        /// </summary>
        public MembershipPlanDto MembershipPlan { get; set; }
    }
}
