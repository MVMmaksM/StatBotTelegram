using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatBotTelegram.Migrations
{
    /// <inheritdoc />
    public partial class addFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employees_departments_DepartmentId",
                table: "employees");

            migrationBuilder.DropForeignKey(
                name: "FK_forms_periodicity_forms_PeriodicityFormId",
                table: "forms");

            migrationBuilder.RenameColumn(
                name: "PeriodicityFormId",
                table: "forms",
                newName: "periodicity_form_id");

            migrationBuilder.RenameIndex(
                name: "IX_forms_PeriodicityFormId",
                table: "forms",
                newName: "IX_forms_periodicity_form_id");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "employees",
                newName: "department_id");

            migrationBuilder.RenameIndex(
                name: "IX_employees_DepartmentId",
                table: "employees",
                newName: "IX_employees_department_id");

            migrationBuilder.AlterColumn<int>(
                name: "department_id",
                table: "employees",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_departments",
                table: "employees",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_periodicity_forms",
                table: "forms",
                column: "periodicity_form_id",
                principalTable: "periodicity_forms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_departments",
                table: "employees");

            migrationBuilder.DropForeignKey(
                name: "fk_periodicity_forms",
                table: "forms");

            migrationBuilder.RenameColumn(
                name: "periodicity_form_id",
                table: "forms",
                newName: "PeriodicityFormId");

            migrationBuilder.RenameIndex(
                name: "IX_forms_periodicity_form_id",
                table: "forms",
                newName: "IX_forms_PeriodicityFormId");

            migrationBuilder.RenameColumn(
                name: "department_id",
                table: "employees",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_employees_department_id",
                table: "employees",
                newName: "IX_employees_DepartmentId");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "employees",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_employees_departments_DepartmentId",
                table: "employees",
                column: "DepartmentId",
                principalTable: "departments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_forms_periodicity_forms_PeriodicityFormId",
                table: "forms",
                column: "PeriodicityFormId",
                principalTable: "periodicity_forms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
