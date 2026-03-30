using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Admins",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "AdminId",
                keyValue: new Guid("c95a5ea7-0956-49d4-8047-68b49ad54fdc"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: new Guid("d8b84401-eba8-4a64-9f19-23f3344e0e82"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: new Guid("f88f5b24-d669-49e3-b21b-072a50c08bc3"),
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Admins");
        }
    }
}
