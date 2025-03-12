using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOnDeleteToCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipPlans_Admins_AdminID",
                table: "MembershipPlans");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipPlans_Admins_AdminID",
                table: "MembershipPlans",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipPlans_Admins_AdminID",
                table: "MembershipPlans");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipPlans_Admins_AdminID",
                table: "MembershipPlans",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
