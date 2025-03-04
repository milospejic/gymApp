using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Index(nameof(PlanName), IsUnique = true, Name = "IX_Plan_Name_Unique")]
    public class MembershipPlan
    {
        [Key]
        public Guid PlanID { get; set; }

        [Required(ErrorMessage = "Plan name is required")]
        [StringLength(100, ErrorMessage = "Plan name cannot exceed 100 characters")]
        public string PlanName { get; set; }

        [Required(ErrorMessage = "Plan description is required")]
        [StringLength(500, ErrorMessage = "Plan description cannot exceed 500 characters")]
        public string PlanDescription { get; set; }

        [Required(ErrorMessage = "Plan price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double PlanPrice { get; set; }

        [Required(ErrorMessage = "Admin ID is required")]
        public Guid AdminID { get; set; }

        [ForeignKey("AdminID")]
        public virtual Admin Admin { get; set; }
    }
}