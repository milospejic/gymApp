using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Backend.Data.Context
{
    public class MyDbContext : DbContext
    {

        private readonly IConfiguration configuration;

        public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MembershipPlan> MembershipPlans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("GymDBConnection"));

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Member>()
                .HasOne(m => m.Membership)
                .WithMany()
                .HasForeignKey(m => m.MembershipID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Membership>()
                .HasOne(m => m.MembershipPlan)
                .WithMany()
                .HasForeignKey(m => m.MembershipPlanID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MembershipPlan>()
                .HasOne(mp => mp.Admin)
                .WithMany()
                .HasForeignKey(mp => mp.AdminID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.AdminEmail)
                .IsUnique();

            modelBuilder.Entity<Member>()
                .HasIndex(m => m.MemberEmail)
                .IsUnique();

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    AdminId = Guid.Parse("c95a5ea7-0956-49d4-8047-68b49ad54fdc"),
                    AdminName = "Petar",
                    AdminSurname = "Petrovic",
                    AdminPhone = "0649459884",
                    AdminEmail = "petar@example.com",
                    AdminHashedPassword = "Petar123!" 

                }
            );
            modelBuilder.Entity<MembershipPlan>().HasData(
                new MembershipPlan
                {
                    PlanID = Guid.Parse("b1780029-6d8d-45cc-ab53-e3d20433007b"),
                    PlanName = "Standard",
                    PlanDescription = "Gym",
                    PlanPrice = 30,
                    AdminID = Guid.Parse("c95a5ea7-0956-49d4-8047-68b49ad54fdc")

                },
                new MembershipPlan
                {
                    PlanID = Guid.Parse("87272d68-35fd-4bf5-af55-5f0daa5bada8"),
                    PlanName = "Silver",
                    PlanDescription = "Gym + Spa",
                    PlanPrice = 45,
                    AdminID = Guid.Parse("c95a5ea7-0956-49d4-8047-68b49ad54fdc")

                }
            );

            modelBuilder.Entity<Membership>().HasData(
                new Membership
                {
                    MembershipID = Guid.Parse("48923344-0974-45f1-8d72-25030d19437e"),
                    MembershipFrom = new DateTime(2025, 2, 21),
                    MembershipTo = new DateTime(2025, 3, 21),
                    MembershipStatus = "Active",
                    PlanDuration = Enums.Duration.OneMonth,
                    MembershipFee = 30,
                    IsFeePaid = false,
                    MembershipPlanID = Guid.Parse("b1780029-6d8d-45cc-ab53-e3d20433007b")
                },
                new Membership
                {
                    MembershipID = Guid.Parse("ac6e2085-57ec-4c1e-a34c-42408b9daebe"),
                    MembershipFrom = new DateTime(2025, 2, 19),
                    MembershipTo = new DateTime(2025, 3, 19),
                    MembershipStatus = "Active",
                    PlanDuration = Enums.Duration.OneMonth,
                    MembershipFee = 30,
                    IsFeePaid = false,
                    MembershipPlanID = Guid.Parse("b1780029-6d8d-45cc-ab53-e3d20433007b")

                }
            );

            modelBuilder.Entity<Member>().HasData(
                new Member
                {
                    MemberId = Guid.Parse("d8b84401-eba8-4a64-9f19-23f3344e0e82"),
                    MemberName = "Jovan",
                    MemberSurname = "Jovanovic",
                    MemberPhone = "0648751234",
                    MemberEmail = "jovan@example.com",
                    MemberHashedPassword = "Jovan123!",
                    MembershipID = Guid.Parse("48923344-0974-45f1-8d72-25030d19437e")

                },
                new Member
                {
                    MemberId = Guid.Parse("f88f5b24-d669-49e3-b21b-072a50c08bc3"),
                    MemberName = "Masa",
                    MemberSurname = "Lukic",
                    MemberPhone = "0645731988",
                    MemberEmail = "masa@example.com",
                    MemberHashedPassword = "Masa123!",
                    MembershipID = Guid.Parse("ac6e2085-57ec-4c1e-a34c-42408b9daebe")

                }
            );

        }
    }
}
