﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persisitence.Database;

#nullable disable

namespace StatBotTelegram.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250304112541_InitialCreate")]
    partial class InitialCreate
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
                        .HasColumnType("text")
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
                        .HasColumnType("integer");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("firstname");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lastname");

                    b.Property<long>("Phone")
                        .HasColumnType("bigint")
                        .HasColumnName("phone");

                    b.Property<string>("SurName")
                        .HasColumnType("text")
                        .HasColumnName("surname");

                    b.HasKey("Id")
                        .HasName("pk_id_employees");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("Phone")
                        .IsUnique()
                        .HasDatabaseName("inx_phone_employees");

                    b.ToTable("employees", (string)null);
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
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Okud")
                        .HasColumnType("integer")
                        .HasColumnName("okud");

                    b.Property<int>("PeriodicityFormId")
                        .HasColumnType("integer");

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
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_id_periodicity_forms");

                    b.ToTable("periodicity_forms", (string)null);
                });

            modelBuilder.Entity("EmployeeForm", b =>
                {
                    b.Property<int>("EmployeesId")
                        .HasColumnType("integer");

                    b.Property<int>("FormsId")
                        .HasColumnType("integer");

                    b.HasKey("EmployeesId", "FormsId");

                    b.HasIndex("FormsId");

                    b.ToTable("EmployeeForm");
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.HasOne("Domain.Entities.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Domain.Entities.Form", b =>
                {
                    b.HasOne("Domain.Entities.PeriodicityForm", "PeriodicityForm")
                        .WithMany()
                        .HasForeignKey("PeriodicityFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PeriodicityForm");
                });

            modelBuilder.Entity("EmployeeForm", b =>
                {
                    b.HasOne("Domain.Entities.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Form", null)
                        .WithMany()
                        .HasForeignKey("FormsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
