using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: new Guid("48923344-0974-45f1-8d72-25030d19437e"),
                columns: new[] { "MembershipFrom", "MembershipTo" },
                values: new object[] { new DateTime(2025, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: new Guid("ac6e2085-57ec-4c1e-a34c-42408b9daebe"),
                columns: new[] { "MembershipFrom", "MembershipTo" },
                values: new object[] { new DateTime(2025, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: new Guid("48923344-0974-45f1-8d72-25030d19437e"),
                columns: new[] { "MembershipFrom", "MembershipTo" },
                values: new object[] { new DateTime(2025, 2, 21, 19, 0, 7, 79, DateTimeKind.Local).AddTicks(5208), new DateTime(2025, 2, 21, 19, 0, 7, 81, DateTimeKind.Local).AddTicks(9211) });

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: new Guid("ac6e2085-57ec-4c1e-a34c-42408b9daebe"),
                columns: new[] { "MembershipFrom", "MembershipTo" },
                values: new object[] { new DateTime(2025, 2, 21, 19, 0, 7, 82, DateTimeKind.Local).AddTicks(1305), new DateTime(2025, 2, 21, 19, 0, 7, 82, DateTimeKind.Local).AddTicks(1321) });
        }
    }
}
