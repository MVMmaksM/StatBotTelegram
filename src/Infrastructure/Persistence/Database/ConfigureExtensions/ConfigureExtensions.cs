using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persisitence.Database.ConfigureExtensions;

public static class ConfigureExtensions
{
    public static void ConfigureForms(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Form>()
            .ToTable("forms");
        modelBuilder
            .Entity<Form>()
            .Property(f => f.Id)
            .HasColumnName("id");
        modelBuilder
            .Entity<Form>()
            .Property(f => f.Name)
            .HasColumnName("name")
            .HasMaxLength(128);
        modelBuilder
            .Entity<Form>()
            .Property(f => f.Okud)
            .HasColumnName("okud");
        modelBuilder
            .Entity<Form>()
            .Property(f => f.PeriodicityFormId)
            .HasColumnName("periodicity_form_id");
        
        modelBuilder
            .Entity<Form>()
            .HasKey(f => f.Id)
            .HasName("pk_id_forms");
        modelBuilder
            .Entity<Form>()
            .HasIndex(f => f.Okud)
            .IsUnique()
            .HasDatabaseName("inx_okud_forms");

        modelBuilder
            .Entity<Form>()
            .HasOne(f => f.PeriodicityForm)
            .WithMany(p => p.Forms)
            .HasForeignKey(f => f.PeriodicityFormId)
            .HasConstraintName("fk_periodicity_forms");
    }
    
    public static void ConfigureDepartment(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Department>()
            .ToTable("departments");
        modelBuilder
            .Entity<Department>()
            .Property(d => d.Id)
            .HasColumnName("id");
        modelBuilder
            .Entity<Department>()
            .Property(d => d.Name)
            .HasColumnName("name")
            .HasMaxLength(512);
        
        modelBuilder
            .Entity<Department>()
            .HasKey(d => d.Id)
            .HasName("pk_id_departments");
    }
    
    public static void ConfigurePeriodicityForm(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<PeriodicityForm>()
            .ToTable("periodicity_forms");
        modelBuilder
            .Entity<PeriodicityForm>()
            .Property(d => d.Id)
            .HasColumnName("id");
        modelBuilder
            .Entity<PeriodicityForm>()
            .Property(d => d.Name)
            .HasColumnName("name")
            .HasMaxLength(128);
        
        modelBuilder
            .Entity<PeriodicityForm>()
            .HasKey(d => d.Id)
            .HasName("pk_id_periodicity_forms");
    }
    
    public static void ConfigureEmployee(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Employee>()
            .ToTable("employees");
        modelBuilder
            .Entity<Employee>()
            .Property(e => e.Id)
            .HasColumnName("id");
        modelBuilder
            .Entity<Employee>()
            .Property(e => e.FirstName)
            .HasColumnName("firstname")
            .HasMaxLength(128);
        modelBuilder
            .Entity<Employee>()
            .Property(e => e.LastName)
            .HasColumnName("lastname")
            .HasMaxLength(128);
        modelBuilder
            .Entity<Employee>()
            .Property(e => e.SurName)
            .HasColumnName("surname")
            .HasMaxLength(128);
        modelBuilder
            .Entity<Employee>()
            .Property(e => e.Phone)
            .HasColumnName("phone");
        modelBuilder
            .Entity<Employee>()
            .Property(e => e.DepartmentId)
            .HasColumnName("department_id");
        
        modelBuilder
            .Entity<Employee>()
            .HasKey(d => d.Id)
            .HasName("pk_id_employees");
        modelBuilder
            .Entity<Employee>()
            .HasIndex(d => d.Phone)
            .IsUnique()
            .HasDatabaseName("inx_phone_employees");
        
        modelBuilder
            .Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(d => d.DepartmentId)
            .HasConstraintName("fk_departments");
    }

    public static void ConfigureEmployeeForm(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Employee>()
            .HasMany(e => e.Forms)
            .WithMany(f => f.Employees)
            .UsingEntity(t => t.ToTable("employees_forms"));
    }
}