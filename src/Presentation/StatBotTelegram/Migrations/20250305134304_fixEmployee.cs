using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatBotTelegram.Migrations
{
    /// <inheritdoc />
    public partial class fixEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "inx_phone_employees",
                table: "employees");

            migrationBuilder.CreateIndex(
                name: "inx_phone_employees",
                table: "employees",
                column: "phone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "inx_phone_employees",
                table: "employees");

            migrationBuilder.CreateIndex(
                name: "inx_phone_employees",
                table: "employees",
                column: "phone",
                unique: true);
        }
    }
}
