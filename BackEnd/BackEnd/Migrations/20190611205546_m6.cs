using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class m6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Photos",
                table: "Pets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photos",
                table: "Pets");
        }
    }
}
