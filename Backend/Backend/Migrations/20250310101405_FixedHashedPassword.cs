using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class FixedHashedPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "AdminId",
                keyValue: new Guid("c95a5ea7-0956-49d4-8047-68b49ad54fdc"),
                column: "AdminHashedPassword",
                value: "$2a$10$CumaLEDEtSsYhcXDZPOnnOxu7.xxZco7ViMg.7m6mFeRkAe4sGzCS");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: new Guid("d8b84401-eba8-4a64-9f19-23f3344e0e82"),
                column: "MemberHashedPassword",
                value: "$2a$10$rOVEpsrnqQYlzpRizY/.XOGfB7ztiqocgS6F3sxQeumTxRWQHWRja");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: new Guid("f88f5b24-d669-49e3-b21b-072a50c08bc3"),
                column: "MemberHashedPassword",
                value: "$2a$10$auEZi85mbEUQ.UwxAg3aN.CBE.of6yvuMrFNsYtYJE9WBZFFmteHa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "AdminId",
                keyValue: new Guid("c95a5ea7-0956-49d4-8047-68b49ad54fdc"),
                column: "AdminHashedPassword",
                value: "Petar123!");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: new Guid("d8b84401-eba8-4a64-9f19-23f3344e0e82"),
                column: "MemberHashedPassword",
                value: "Jovan123!");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: new Guid("f88f5b24-d669-49e3-b21b-072a50c08bc3"),
                column: "MemberHashedPassword",
                value: "Masa123!");
        }
    }
}
