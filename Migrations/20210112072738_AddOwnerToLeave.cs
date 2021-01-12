using Microsoft.EntityFrameworkCore.Migrations;

namespace ems.Migrations
{
    public partial class AddOwnerToLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "leave",
                type: "varchar(95)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_leave_OwnerId",
                table: "leave",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_leave_AspNetUsers_OwnerId",
                table: "leave",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leave_AspNetUsers_OwnerId",
                table: "leave");

            migrationBuilder.DropIndex(
                name: "IX_leave_OwnerId",
                table: "leave");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "leave");
        }
    }
}
