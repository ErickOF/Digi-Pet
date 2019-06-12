using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class m5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pet_Petowner_PetOwnerId",
                table: "Pet");

            migrationBuilder.DropForeignKey(
                name: "FK_Petowner_Users_UserId",
                table: "Petowner");

            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Pet_PetId",
                table: "Walks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Petowner",
                table: "Petowner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pet",
                table: "Pet");

            migrationBuilder.RenameTable(
                name: "Petowner",
                newName: "Owners");

            migrationBuilder.RenameTable(
                name: "Pet",
                newName: "Pets");

            migrationBuilder.RenameIndex(
                name: "IX_Petowner_UserId",
                table: "Owners",
                newName: "IX_Owners_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Pet_PetOwnerId",
                table: "Pets",
                newName: "IX_Pets_PetOwnerId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "Pets",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Pets",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owners",
                table: "Owners",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pets",
                table: "Pets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Users_UserId",
                table: "Owners",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Owners_PetOwnerId",
                table: "Pets",
                column: "PetOwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Pets_PetId",
                table: "Walks",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Users_UserId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Owners_PetOwnerId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Walks_Pets_PetId",
                table: "Walks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pets",
                table: "Pets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Owners",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Pets",
                newName: "Pet");

            migrationBuilder.RenameTable(
                name: "Owners",
                newName: "Petowner");

            migrationBuilder.RenameIndex(
                name: "IX_Pets_PetOwnerId",
                table: "Pet",
                newName: "IX_Pet_PetOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Owners_UserId",
                table: "Petowner",
                newName: "IX_Petowner_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "Pet",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Pet",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pet",
                table: "Pet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Petowner",
                table: "Petowner",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_Petowner_PetOwnerId",
                table: "Pet",
                column: "PetOwnerId",
                principalTable: "Petowner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Petowner_Users_UserId",
                table: "Petowner",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_Pet_PetId",
                table: "Walks",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
