using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatBotTelegram.Migrations
{
    /// <inheritdoc />
    public partial class fix_employee_form : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_forms_forms_fomr_id",
                table: "employee_forms");

            migrationBuilder.RenameColumn(
                name: "fomr_id",
                table: "employee_forms",
                newName: "form_id");

            migrationBuilder.RenameIndex(
                name: "IX_employee_forms_fomr_id",
                table: "employee_forms",
                newName: "IX_employee_forms_form_id");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_forms_forms_form_id",
                table: "employee_forms",
                column: "form_id",
                principalTable: "forms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_forms_forms_form_id",
                table: "employee_forms");

            migrationBuilder.RenameColumn(
                name: "form_id",
                table: "employee_forms",
                newName: "fomr_id");

            migrationBuilder.RenameIndex(
                name: "IX_employee_forms_form_id",
                table: "employee_forms",
                newName: "IX_employee_forms_fomr_id");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_forms_forms_fomr_id",
                table: "employee_forms",
                column: "fomr_id",
                principalTable: "forms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
