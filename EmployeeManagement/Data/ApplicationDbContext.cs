// Data/ApplicationDbContext.cs
using EmployeeManagement.Model;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<EmployeeDto> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeDto>()
            .HasKey(e => e.Id);

        // Make the combination of first name, last name and email address unique.
        modelBuilder.Entity<EmployeeDto>()
            .HasIndex(e => new { e.FirstName, e.LastName, e.Email, e.Age, e.Address })
            .IsUnique();
    }
}
