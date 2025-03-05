using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatBotTelegram.Migrations
{
    /// <inheritdoc />
    public partial class fixPhoneEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "employees",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "phone",
                table: "employees",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);
        }
    }
}
