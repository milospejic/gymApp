using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminHashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberHashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MembershipID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                });

            migrationBuilder.CreateTable(
                name: "MembershipPlans",
                columns: table => new
                {
                    PlanID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanPrice = table.Column<double>(type: "float", nullable: false),
                    AdminID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPlans", x => x.PlanID);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    MembershipID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MembershipTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MembershipStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanDuration = table.Column<int>(type: "int", nullable: false),
                    MembershipFee = table.Column<double>(type: "float", nullable: false),
                    IsFeePaid = table.Column<bool>(type: "bit", nullable: false),
                    MembershipPlanID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.MembershipID);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "AdminId", "AdminEmail", "AdminHashedPassword", "AdminName", "AdminPhone", "AdminSurname" },
                values: new object[] { new Guid("c95a5ea7-0956-49d4-8047-68b49ad54fdc"), "petar@example.com", "petar123", "Petar", "0649459884", "Petrovic" });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "MemberEmail", "MemberHashedPassword", "MemberName", "MemberPhone", "MemberSurname", "MembershipID" },
                values: new object[,]
                {
                    { new Guid("d8b84401-eba8-4a64-9f19-23f3344e0e82"), "jovan@example.com", "jovan123", "Jovan", "0648751234", "Jovanovic", new Guid("48923344-0974-45f1-8d72-25030d19437e") },
                    { new Guid("f88f5b24-d669-49e3-b21b-072a50c08bc3"), "masa@example.com", "masa1234", "Masa", "0645731988", "Lukic", new Guid("ac6e2085-57ec-4c1e-a34c-42408b9daebe") }
                });

            migrationBuilder.InsertData(
                table: "MembershipPlans",
                columns: new[] { "PlanID", "AdminID", "PlanDescription", "PlanName", "PlanPrice" },
                values: new object[,]
                {
                    { new Guid("87272d68-35fd-4bf5-af55-5f0daa5bada8"), new Guid("c95a5ea7-0956-49d4-8047-68b49ad54fdc"), "Gym + Spa for 30 days", "Silver", 45.0 },
                    { new Guid("b1780029-6d8d-45cc-ab53-e3d20433007b"), new Guid("c95a5ea7-0956-49d4-8047-68b49ad54fdc"), "Gym for 30 days", "Standard", 30.0 }
                });

            migrationBuilder.InsertData(
                table: "Memberships",
                columns: new[] { "MembershipID", "IsFeePaid", "MembershipFee", "MembershipFrom", "MembershipPlanID", "MembershipStatus", "MembershipTo", "PlanDuration" },
                values: new object[,]
                {
                    { new Guid("48923344-0974-45f1-8d72-25030d19437e"), false, 30.0, new DateTime(2025, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b1780029-6d8d-45cc-ab53-e3d20433007b"), "Active", new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { new Guid("ac6e2085-57ec-4c1e-a34c-42408b9daebe"), false, 30.0, new DateTime(2025, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b1780029-6d8d-45cc-ab53-e3d20433007b"), "Active", new DateTime(2025, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "MembershipPlans");

            migrationBuilder.DropTable(
                name: "Memberships");
        }
    }
}
