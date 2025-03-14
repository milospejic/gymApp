using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    /// <summary>
    /// Represents a membership plan with pricing and administrative assignment.
    /// </summary>
    [Index(nameof(PlanName), IsUnique = true, Name = "IX_Plan_Name_Unique")]
    public class MembershipPlan
    {
        /// <summary>
        /// Unique identifier for the membership plan.
        /// </summary>
        [Key]
        public Guid PlanID { get; set; }

        /// <summary>
        /// Name of the membership plan.
        /// </summary>
        [Required(ErrorMessage = "Plan name is required")]
        [StringLength(100, ErrorMessage = "Plan name cannot exceed 100 characters")]
        public string PlanName { get; set; }

        /// <summary>
        /// Description of the membership plan.
        /// </summary>
        [Required(ErrorMessage = "Plan description is required")]
        [StringLength(500, ErrorMessage = "Plan description cannot exceed 500 characters")]
        public string PlanDescription { get; set; }

        /// <summary>
        /// Membership plan fee amount.
        /// </summary>
        [Required(ErrorMessage = "Plan price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double PlanPrice { get; set; }

        /// <summary>
        /// Foreign key referencing the admin responsible for the plan.
        /// </summary>
        [Required(ErrorMessage = "Admin ID is required")]
        public Guid? AdminID { get; set; }

        /// <summary>
        /// Indicates if the membership plan is marked for deletion.
        /// </summary>
        [Required(ErrorMessage = "ForDeletion is required")]
        public bool ForDeletion { get; set; }

        /// <summary>
        /// The admin responsible for the latest changes on the plan.
        /// </summary>
        [ForeignKey("AdminID")]
        public virtual Admin Admin { get; set; }

        /// <summary>
        /// Collection of memberships associated with this plan.
        /// </summary>
        public virtual ICollection<Membership> Memberships { get; set; } = new HashSet<Membership>();

    }
}