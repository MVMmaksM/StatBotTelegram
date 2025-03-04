using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatBotTelegram.Migrations
{
    /// <inheritdoc />
    public partial class updateDep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "departments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "departments");
        }
    }
}
