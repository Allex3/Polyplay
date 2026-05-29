using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolyplayAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMoreForUsernames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameComments_Users_UserId",
                table: "GameComments");

            migrationBuilder.DropIndex(
                name: "IX_GameComments_UserId",
                table: "GameComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GameComments");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "GameComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "GameComments");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GameComments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GameComments_UserId",
                table: "GameComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameComments_Users_UserId",
                table: "GameComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
