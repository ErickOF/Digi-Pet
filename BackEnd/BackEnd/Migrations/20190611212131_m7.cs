using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class m7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Walker_Users_UserId",
                table: "Walker");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Walker",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Walker_UserId1",
                table: "Walker",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Walker_Users_UserId1",
                table: "Walker",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Walker_Users_UserId1",
                table: "Walker");

            migrationBuilder.DropIndex(
                name: "IX_Walker_UserId1",
                table: "Walker");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Walker");

            migrationBuilder.AddForeignKey(
                name: "FK_Walker_Users_UserId",
                table: "Walker",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
