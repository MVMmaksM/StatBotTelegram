using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatBotTelegram.Migrations
{
    /// <inheritdoc />
    public partial class updateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeForm_employees_EmployeesId",
                table: "EmployeeForm");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeForm_forms_FormsId",
                table: "EmployeeForm");

            migrationBuilder.DropForeignKey(
                name: "FK_employees_departments_DepartmentId",
                table: "employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeForm",
                table: "EmployeeForm");

            migrationBuilder.DropColumn(
                name: "Test",
                table: "departments");

            migrationBuilder.RenameTable(
                name: "EmployeeForm",
                newName: "employees_forms");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeForm_FormsId",
                table: "employees_forms",
                newName: "IX_employees_forms_FormsId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "periodicity_forms",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "forms",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "surname",
                table: "employees",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "lastname",
                table: "employees",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "firstname",
                table: "employees",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "employees",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "departments",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employees_forms",
                table: "employees_forms",
                columns: new[] { "EmployeesId", "FormsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_employees_departments_DepartmentId",
                table: "employees",
                column: "DepartmentId",
                principalTable: "departments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_employees_forms_employees_EmployeesId",
                table: "employees_forms",
                column: "EmployeesId",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employees_forms_forms_FormsId",
                table: "employees_forms",
                column: "FormsId",
                principalTable: "forms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employees_departments_DepartmentId",
                table: "employees");

            migrationBuilder.DropForeignKey(
                name: "FK_employees_forms_employees_EmployeesId",
                table: "employees_forms");

            migrationBuilder.DropForeignKey(
                name: "FK_employees_forms_forms_FormsId",
                table: "employees_forms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employees_forms",
                table: "employees_forms");

            migrationBuilder.RenameTable(
                name: "employees_forms",
                newName: "EmployeeForm");

            migrationBuilder.RenameIndex(
                name: "IX_employees_forms_FormsId",
                table: "EmployeeForm",
                newName: "IX_EmployeeForm_FormsId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "periodicity_forms",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "forms",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "surname",
                table: "employees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "lastname",
                table: "employees",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "firstname",
                table: "employees",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "employees",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "departments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "departments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeForm",
                table: "EmployeeForm",
                columns: new[] { "EmployeesId", "FormsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeForm_employees_EmployeesId",
                table: "EmployeeForm",
                column: "EmployeesId",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeForm_forms_FormsId",
                table: "EmployeeForm",
                column: "FormsId",
                principalTable: "forms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employees_departments_DepartmentId",
                table: "employees",
                column: "DepartmentId",
                principalTable: "departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
