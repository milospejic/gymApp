using Backend.Entities;

namespace Backend.Dto.BasicDtos
{
    /// <summary>
    /// Data Transfer Object for Membership Plan entity.
    /// </summary>
    public class MembershipPlanDto
    {
        /// <summary>
        /// Unique identifier for the membership plan.
        /// </summary>
        public Guid PlanID { get; set; }

        /// <summary>
        /// Name of the membership plan.
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// Description of the membership plan.
        /// </summary>
        public string PlanDescription { get; set; }

        /// <summary>
        /// Price of the membership plan.
        /// </summary>
        public double PlanPrice { get; set; }

        /// <summary>
        /// Indicates if the plan is marked for deletion.
        /// </summary>
        public bool ForDeletion { get; set; }

        /// <summary>
        /// Unique identifier of the admin who manages the plan.
        /// </summary>
        public Guid? AdminID { get; set; }

        /// <summary>
        /// Admin details associated with this plan.
        /// </summary>
        public AdminDto Admin { get; set; }

        /// <summary>
        /// Collection of memberships associated with this plan.
        /// </summary>
        public virtual ICollection<Membership> Memberships { get; set; } = new HashSet<Membership>();
    }
}