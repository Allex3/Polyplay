using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolyplayAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaliciousActivityLog_Users_UserId",
                table: "MaliciousActivityLog");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "MaliciousActivityLog",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "MaliciousActivityLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MaliciousActivityLog_Users_UserId",
                table: "MaliciousActivityLog",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaliciousActivityLog_Users_UserId",
                table: "MaliciousActivityLog");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "MaliciousActivityLog");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "MaliciousActivityLog",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MaliciousActivityLog_Users_UserId",
                table: "MaliciousActivityLog",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
