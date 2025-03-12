﻿// <auto-generated />
using System;
using Backend.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20250312083317_ChangedOnDeleteToCascade")]
    partial class ChangedOnDeleteToCascade
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Backend.Entities.Admin", b =>
                {
                    b.Property<Guid>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdminEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AdminHashedPassword")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AdminName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("AdminPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("AdminSurname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AdminId");

                    b.HasIndex("AdminEmail")
                        .IsUnique();

                    b.HasIndex(new[] { "AdminEmail" }, "IX_Admin_Email_Unique")
                        .IsUnique();

                    b.ToTable("Admins");

                    b.HasData(
                        new
                        {
                            AdminId = new Guid("c95a5ea7-0956-49d4-8047-68b49ad54fdc"),
                            AdminEmail = "petar@example.com",
                            AdminHashedPassword = "$2a$10$CumaLEDEtSsYhcXDZPOnnOxu7.xxZco7ViMg.7m6mFeRkAe4sGzCS",
                            AdminName = "Petar",
                            AdminPhone = "0649459884",
                            AdminSurname = "Petrovic"
                        });
                });

            modelBuilder.Entity("Backend.Entities.Member", b =>
                {
                    b.Property<Guid>("MemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MemberEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("MemberHashedPassword")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("MemberName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MemberPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MemberSurname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("MembershipID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MemberId");

                    b.HasIndex("MemberEmail")
                        .IsUnique();

                    b.HasIndex("MembershipID");

                    b.HasIndex(new[] { "MemberEmail" }, "IX_Member_Email_Unique")
                        .IsUnique();

                    b.ToTable("Members");

                    b.HasData(
                        new
                        {
                            MemberId = new Guid("d8b84401-eba8-4a64-9f19-23f3344e0e82"),
                            MemberEmail = "jovan@example.com",
                            MemberHashedPassword = "$2a$10$rOVEpsrnqQYlzpRizY/.XOGfB7ztiqocgS6F3sxQeumTxRWQHWRja",
                            MemberName = "Jovan",
                            MemberPhone = "0648751234",
                            MemberSurname = "Jovanovic",
                            MembershipID = new Guid("48923344-0974-45f1-8d72-25030d19437e")
                        },
                        new
                        {
                            MemberId = new Guid("f88f5b24-d669-49e3-b21b-072a50c08bc3"),
                            MemberEmail = "masa@example.com",
                            MemberHashedPassword = "$2a$10$auEZi85mbEUQ.UwxAg3aN.CBE.of6yvuMrFNsYtYJE9WBZFFmteHa",
                            MemberName = "Masa",
                            MemberPhone = "0645731988",
                            MemberSurname = "Lukic",
                            MembershipID = new Guid("ac6e2085-57ec-4c1e-a34c-42408b9daebe")
                        });
                });

            modelBuilder.Entity("Backend.Entities.Membership", b =>
                {
                    b.Property<Guid>("MembershipID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsFeePaid")
                        .HasColumnType("bit");

                    b.Property<double>("MembershipFee")
                        .HasColumnType("float");

                    b.Property<DateTime>("MembershipFrom")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MembershipPlanID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("MembershipPlanPlanID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("MembershipTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("PlanDuration")
                        .HasColumnType("int");

                    b.HasKey("MembershipID");

                    b.HasIndex("MembershipPlanID");

                    b.HasIndex("MembershipPlanPlanID");

                    b.ToTable("Memberships");

                    b.HasData(
                        new
                        {
                            MembershipID = new Guid("48923344-0974-45f1-8d72-25030d19437e"),
                            IsFeePaid = false,
                            MembershipFee = 30.0,
                            MembershipFrom = new DateTime(2025, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MembershipPlanID = new Guid("b1780029-6d8d-45cc-ab53-e3d20433007b"),
                            MembershipTo = new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PlanDuration = 1
                        },
                        new
                        {
                            MembershipID = new Guid("ac6e2085-57ec-4c1e-a34c-42408b9daebe"),
                            IsFeePaid = false,
                            MembershipFee = 30.0,
                            MembershipFrom = new DateTime(2025, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MembershipPlanID = new Guid("b1780029-6d8d-45cc-ab53-e3d20433007b"),
                            MembershipTo = new DateTime(2025, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PlanDuration = 1
                        });
                });

            modelBuilder.Entity("Backend.Entities.MembershipPlan", b =>
                {
                    b.Property<Guid>("PlanID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AdminID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("ForDeletion")
                        .HasColumnType("bit");

                    b.Property<string>("PlanDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PlanName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("PlanPrice")
                        .HasColumnType("float");

                    b.HasKey("PlanID");

                    b.HasIndex("AdminID");

                    b.HasIndex(new[] { "PlanName" }, "IX_Plan_Name_Unique")
                        .IsUnique();

                    b.ToTable("MembershipPlans");

                    b.HasData(
                        new
                        {
                            PlanID = new Guid("b1780029-6d8d-45cc-ab53-e3d20433007b"),
                            AdminID = new Guid("c95a5ea7-0956-49d4-8047-68b49ad54fdc"),
                            ForDeletion = false,
                            PlanDescription = "Gym",
                            PlanName = "Standard",
                            PlanPrice = 30.0
                        },
                        new
                        {
                            PlanID = new Guid("87272d68-35fd-4bf5-af55-5f0daa5bada8"),
                            AdminID = new Guid("c95a5ea7-0956-49d4-8047-68b49ad54fdc"),
                            ForDeletion = false,
                            PlanDescription = "Gym + Spa",
                            PlanName = "Silver",
                            PlanPrice = 45.0
                        });
                });

            modelBuilder.Entity("Backend.Entities.Member", b =>
                {
                    b.HasOne("Backend.Entities.Membership", "Membership")
                        .WithMany()
                        .HasForeignKey("MembershipID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Membership");
                });

            modelBuilder.Entity("Backend.Entities.Membership", b =>
                {
                    b.HasOne("Backend.Entities.MembershipPlan", "MembershipPlan")
                        .WithMany()
                        .HasForeignKey("MembershipPlanID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Backend.Entities.MembershipPlan", null)
                        .WithMany("Memberships")
                        .HasForeignKey("MembershipPlanPlanID");

                    b.Navigation("MembershipPlan");
                });

            modelBuilder.Entity("Backend.Entities.MembershipPlan", b =>
                {
                    b.HasOne("Backend.Entities.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("Backend.Entities.MembershipPlan", b =>
                {
                    b.Navigation("Memberships");
                });
#pragma warning restore 612, 618
        }
    }
}
