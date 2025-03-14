﻿using Backend.Entities;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Context
{
    /// <summary>
    /// Represents the database context for the application.
    /// It provides access to all the entity sets and configures database relationships.
    /// </summary>
    public class MyDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the database context.</param>
        /// <param name="configuration">The application configuration containing database connection strings.</param>
        public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets or sets the members in the system.
        /// </summary>
        public DbSet<Member> Members { get; set; }

        /// <summary>
        /// Gets or sets the admins managing the system.
        /// </summary>
        public DbSet<Admin> Admins { get; set; }

        /// <summary>
        /// Gets or sets the memberships associated with members.
        /// </summary>
        public DbSet<Membership> Memberships { get; set; }

        /// <summary>
        /// Gets or sets the membership plans available for members.
        /// </summary>
        public DbSet<MembershipPlan> MembershipPlans { get; set; }

        /// <summary>
        /// Configures the database connection using the connection string from the configuration.
        /// </summary>
        /// <param name="optionsBuilder">The options builder for configuring the database context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("GymDBConnection"));
        }

        /// <summary>
        /// Configures the entity relationships, constraints, and seed data for the database.
        /// </summary>
        /// <param name="modelBuilder">The model builder for defining entity configurations.</param>
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
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MembershipPlan>()
                .HasOne(mp => mp.Admin)
                .WithMany()
                .HasForeignKey(mp => mp.AdminID)
                .OnDelete(DeleteBehavior.Cascade);

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
                    AdminHashedPassword = "$2a$10$CumaLEDEtSsYhcXDZPOnnOxu7.xxZco7ViMg.7m6mFeRkAe4sGzCS"
                }
            );

            modelBuilder.Entity<MembershipPlan>().HasData(
                new MembershipPlan
                {
                    PlanID = Guid.Parse("b1780029-6d8d-45cc-ab53-e3d20433007b"),
                    PlanName = "Standard",
                    PlanDescription = "Gym",
                    PlanPrice = 30,
                    ForDeletion = false,
                    AdminID = Guid.Parse("c95a5ea7-0956-49d4-8047-68b49ad54fdc")
                },
                new MembershipPlan
                {
                    PlanID = Guid.Parse("87272d68-35fd-4bf5-af55-5f0daa5bada8"),
                    PlanName = "Silver",
                    PlanDescription = "Gym + Spa",
                    PlanPrice = 45,
                    ForDeletion = false,
                    AdminID = Guid.Parse("c95a5ea7-0956-49d4-8047-68b49ad54fdc")
                }
            );

            modelBuilder.Entity<Membership>().HasData(
                new Membership
                {
                    MembershipID = Guid.Parse("48923344-0974-45f1-8d72-25030d19437e"),
                    MembershipFrom = new DateTime(2025, 2, 21),
                    MembershipTo = new DateTime(2025, 3, 21),
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
                    MemberHashedPassword = "$2a$10$rOVEpsrnqQYlzpRizY/.XOGfB7ztiqocgS6F3sxQeumTxRWQHWRja",
                    MembershipID = Guid.Parse("48923344-0974-45f1-8d72-25030d19437e")
                },
                new Member
                {
                    MemberId = Guid.Parse("f88f5b24-d669-49e3-b21b-072a50c08bc3"),
                    MemberName = "Masa",
                    MemberSurname = "Lukic",
                    MemberPhone = "0645731988",
                    MemberEmail = "masa@example.com",
                    MemberHashedPassword = "$2a$10$auEZi85mbEUQ.UwxAg3aN.CBE.of6yvuMrFNsYtYJE9WBZFFmteHa",
                    MembershipID = Guid.Parse("ac6e2085-57ec-4c1e-a34c-42408b9daebe")
                }
            );
        }
    }
}
