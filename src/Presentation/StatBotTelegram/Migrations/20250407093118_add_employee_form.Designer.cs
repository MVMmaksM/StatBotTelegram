﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence.Database;

#nullable disable

namespace StatBotTelegram.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250407093118_add_employee_form")]
    partial class add_employee_form
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_id_departments");

                    b.ToTable("departments", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer")
                        .HasColumnName("department_id");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("firstname");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("lastname");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)")
                        .HasColumnName("phone");

                    b.Property<string>("SurName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("surname");

                    b.HasKey("Id")
                        .HasName("pk_id_employees");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("Phone")
                        .HasDatabaseName("inx_phone_employees");

                    b.ToTable("employees", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.EmployeeForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer")
                        .HasColumnName("employee_id");

                    b.Property<int>("FormId")
                        .HasColumnType("integer")
                        .HasColumnName("form_id");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("FormId");

                    b.ToTable("EmployeeForm");
                });

            modelBuilder.Entity("Domain.Entities.Form", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name");

                    b.Property<int>("Okud")
                        .HasColumnType("integer")
                        .HasColumnName("okud");

                    b.Property<int>("PeriodicityFormId")
                        .HasColumnType("integer")
                        .HasColumnName("periodicity_form_id");

                    b.HasKey("Id")
                        .HasName("pk_id_forms");

                    b.HasIndex("Okud")
                        .IsUnique()
                        .HasDatabaseName("inx_okud_forms");

                    b.HasIndex("PeriodicityFormId");

                    b.ToTable("forms", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.PeriodicityForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_id_periodicity_forms");

                    b.ToTable("periodicity_forms", (string)null);
                });

            modelBuilder.Entity("employee_forms", b =>
                {
                    b.Property<int>("employee_id")
                        .HasColumnType("integer");

                    b.Property<int>("fomr_id")
                        .HasColumnType("integer");

                    b.HasKey("employee_id", "fomr_id");

                    b.HasIndex("fomr_id");

                    b.ToTable("employee_forms", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.HasOne("Domain.Entities.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_departments");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Domain.Entities.EmployeeForm", b =>
                {
                    b.HasOne("Domain.Entities.Employee", "Employee")
                        .WithMany("EmployeesForms")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Form", "Form")
                        .WithMany("EmployeesForms")
                        .HasForeignKey("FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Form");
                });

            modelBuilder.Entity("Domain.Entities.Form", b =>
                {
                    b.HasOne("Domain.Entities.PeriodicityForm", "PeriodicityForm")
                        .WithMany("Forms")
                        .HasForeignKey("PeriodicityFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PeriodicityForm");
                });

            modelBuilder.Entity("employee_forms", b =>
                {
                    b.HasOne("Domain.Entities.Employee", null)
                        .WithMany()
                        .HasForeignKey("employee_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Form", null)
                        .WithMany()
                        .HasForeignKey("fomr_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.Navigation("EmployeesForms");
                });

            modelBuilder.Entity("Domain.Entities.Form", b =>
                {
                    b.Navigation("EmployeesForms");
                });

            modelBuilder.Entity("Domain.Entities.PeriodicityForm", b =>
                {
                    b.Navigation("Forms");
                });
#pragma warning restore 612, 618
        }
    }
}
