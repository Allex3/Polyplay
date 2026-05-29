using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolyplayAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMoreForUsernames2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserActivityLog_Users_UserId",
                table: "UserActivityLog");

            migrationBuilder.DropIndex(
                name: "IX_UserActivityLog_UserId",
                table: "UserActivityLog");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserActivityLog");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserActivityLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserActivityLog");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserActivityLog",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivityLog_UserId",
                table: "UserActivityLog",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivityLog_Users_UserId",
                table: "UserActivityLog",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
