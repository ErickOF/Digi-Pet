using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebApi.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    Canton = table.Column<string>(nullable: false),
                    Email2 = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_Username", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Petowner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Petowner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Petowner_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Walker",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<int>(nullable: false),
                    UserId1 = table.Column<int>(nullable: true),
                    University = table.Column<string>(nullable: false),
                    DoesOtherProvinces = table.Column<bool>(nullable: false),
                    OtherProvinces = table.Column<string[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Walker", x => x.Id);
                    table.UniqueConstraint("AK_Walker_UserId", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Walker_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Race = table.Column<string>(maxLength: 30, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    Size = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PetOwnerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pet_Petowner_PetOwnerId",
                        column: x => x.PetOwnerId,
                        principalTable: "Petowner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Walks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PetId = table.Column<int>(nullable: false),
                    WalkerId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Walks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Walks_Pet_PetId",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Walks_Walker_WalkerId",
                        column: x => x.WalkerId,
                        principalTable: "Walker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportWalk",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    WalkId = table.Column<int>(nullable: false),
                    Stars = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportWalk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportWalk_Walks_WalkId",
                        column: x => x.WalkId,
                        principalTable: "Walks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pet_PetOwnerId",
                table: "Pet",
                column: "PetOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Petowner_UserId",
                table: "Petowner",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportWalk_WalkId",
                table: "ReportWalk",
                column: "WalkId");

            migrationBuilder.CreateIndex(
                name: "IX_Walker_UserId1",
                table: "Walker",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Walks_PetId",
                table: "Walks",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Walks_WalkerId",
                table: "Walks",
                column: "WalkerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportWalk");

            migrationBuilder.DropTable(
                name: "Walks");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropTable(
                name: "Walker");

            migrationBuilder.DropTable(
                name: "Petowner");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
