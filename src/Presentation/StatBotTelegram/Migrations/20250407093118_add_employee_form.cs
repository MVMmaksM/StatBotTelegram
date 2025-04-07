using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StatBotTelegram.Migrations
{
    /// <inheritdoc />
    public partial class add_employee_form : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_periodicity_forms",
                table: "forms");

            migrationBuilder.DropTable(
                name: "employees_forms");

            migrationBuilder.CreateTable(
                name: "employee_forms",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    fomr_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_forms", x => new { x.employee_id, x.fomr_id });
                    table.ForeignKey(
                        name: "FK_employee_forms_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employee_forms_forms_fomr_id",
                        column: x => x.fomr_id,
                        principalTable: "forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    form_id = table.Column<int>(type: "integer", nullable: false),
                    employee_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeForm_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeForm_forms_form_id",
                        column: x => x.form_id,
                        principalTable: "forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employee_forms_fomr_id",
                table: "employee_forms",
                column: "fomr_id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeForm_employee_id",
                table: "EmployeeForm",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeForm_form_id",
                table: "EmployeeForm",
                column: "form_id");

            migrationBuilder.AddForeignKey(
                name: "FK_forms_periodicity_forms_periodicity_form_id",
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
                name: "FK_forms_periodicity_forms_periodicity_form_id",
                table: "forms");

            migrationBuilder.DropTable(
                name: "employee_forms");

            migrationBuilder.DropTable(
                name: "EmployeeForm");

            migrationBuilder.CreateTable(
                name: "employees_forms",
                columns: table => new
                {
                    EmployeesId = table.Column<int>(type: "integer", nullable: false),
                    FormsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees_forms", x => new { x.EmployeesId, x.FormsId });
                    table.ForeignKey(
                        name: "FK_employees_forms_employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employees_forms_forms_FormsId",
                        column: x => x.FormsId,
                        principalTable: "forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employees_forms_FormsId",
                table: "employees_forms",
                column: "FormsId");

            migrationBuilder.AddForeignKey(
                name: "fk_periodicity_forms",
                table: "forms",
                column: "periodicity_form_id",
                principalTable: "periodicity_forms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
