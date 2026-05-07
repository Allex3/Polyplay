using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolyplayAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixedActivityLogForeignkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "UserActivityLog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ActivityId",
                table: "UserActivityLog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
