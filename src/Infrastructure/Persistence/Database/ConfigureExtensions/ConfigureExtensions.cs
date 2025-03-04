using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persisitence.Database.ConfigureExtensions;

public static class ConfigureExtensions
{
    public static void ConfigureForms(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Form>().ToTable("forms");
        modelBuilder.Entity<Form>().Property(f => f.Id).HasColumnName("id");
        modelBuilder.Entity<Form>().Property(f => f.Name).HasColumnName("name");
        modelBuilder.Entity<Form>().Property(f => f.Okud).HasColumnName("okud");
        
        modelBuilder.Entity<Form>().HasKey(f => f.Id).HasName("pk_id_forms");
        modelBuilder.Entity<Form>().HasIndex(f => f.Okud).IsUnique().HasDatabaseName("inx_okud_forms");
    }
    
    public static void ConfigureDepartment(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>().ToTable("departments");
        modelBuilder.Entity<Department>().Property(d => d.Id).HasColumnName("id");
        modelBuilder.Entity<Department>().Property(d => d.Name).HasColumnName("name");
        
        modelBuilder.Entity<Department>().HasKey(d => d.Id).HasName("pk_id_departments");
    }
    
    public static void ConfigurePeriodicityForm(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PeriodicityForm>().ToTable("periodicity_forms");
        modelBuilder.Entity<PeriodicityForm>().Property(d => d.Id).HasColumnName("id");
        modelBuilder.Entity<PeriodicityForm>().Property(d => d.Name).HasColumnName("name");
        
        modelBuilder.Entity<PeriodicityForm>().HasKey(d => d.Id).HasName("pk_id_periodicity_forms");
    }
    
    public static void ConfigureEmployee(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().ToTable("employees");
        modelBuilder.Entity<Employee>().Property(e => e.Id).HasColumnName("id");
        modelBuilder.Entity<Employee>().Property(e => e.FirstName).HasColumnName("firstname");
        modelBuilder.Entity<Employee>().Property(e => e.LastName).HasColumnName("lastname");
        modelBuilder.Entity<Employee>().Property(e => e.SurName).HasColumnName("surname");
        modelBuilder.Entity<Employee>().Property(e => e.Phone).HasColumnName("phone");
        
        modelBuilder.Entity<Employee>().HasKey(d => d.Id).HasName("pk_id_employees");
        modelBuilder.Entity<Employee>().HasIndex(d => d.Phone).IsUnique().HasDatabaseName("inx_phone_employees");
    }
}