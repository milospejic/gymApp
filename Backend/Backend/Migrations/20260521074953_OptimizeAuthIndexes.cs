using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class OptimizeAuthIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Members_MemberEmail",
                table: "Members",
                newName: "IX_Member_MemberEmail");

            migrationBuilder.RenameIndex(
                name: "IX_Admins_AdminEmail",
                table: "Admins",
                newName: "IX_Admin_AdminEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Member_MemberEmail",
                table: "Members",
                newName: "IX_Members_MemberEmail");

            migrationBuilder.RenameIndex(
                name: "IX_Admin_AdminEmail",
                table: "Admins",
                newName: "IX_Admins_AdminEmail");
        }
    }
}
