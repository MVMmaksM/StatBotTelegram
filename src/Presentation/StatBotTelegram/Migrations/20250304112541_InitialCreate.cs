using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StatBotTelegram.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_id_departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "periodicity_forms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_id_periodicity_forms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_id_employees", x => x.id);
                    table.ForeignKey(
                        name: "FK_employees_departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "forms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    okud = table.Column<int>(type: "integer", nullable: false),
                    PeriodicityFormId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_id_forms", x => x.id);
                    table.ForeignKey(
                        name: "FK_forms_periodicity_forms_PeriodicityFormId",
                        column: x => x.PeriodicityFormId,
                        principalTable: "periodicity_forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeForm",
                columns: table => new
                {
                    EmployeesId = table.Column<int>(type: "integer", nullable: false),
                    FormsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeForm", x => new { x.EmployeesId, x.FormsId });
                    table.ForeignKey(
                        name: "FK_EmployeeForm_employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeForm_forms_FormsId",
                        column: x => x.FormsId,
                        principalTable: "forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeForm_FormsId",
                table: "EmployeeForm",
                column: "FormsId");

            migrationBuilder.CreateIndex(
                name: "inx_phone_employees",
                table: "employees",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_DepartmentId",
                table: "employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "inx_okud_forms",
                table: "forms",
                column: "okud",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_forms_PeriodicityFormId",
                table: "forms",
                column: "PeriodicityFormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeForm");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "forms");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "periodicity_forms");
        }
    }
}
