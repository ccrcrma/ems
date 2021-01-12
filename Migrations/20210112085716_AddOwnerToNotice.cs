using Microsoft.EntityFrameworkCore.Migrations;

namespace ems.Migrations
{
    public partial class AddOwnerToNotice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "notice",
                type: "varchar(95)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_notice_OwnerId",
                table: "notice",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_notice_AspNetUsers_OwnerId",
                table: "notice",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notice_AspNetUsers_OwnerId",
                table: "notice");

            migrationBuilder.DropIndex(
                name: "IX_notice_OwnerId",
                table: "notice");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "notice");
        }
    }
}
