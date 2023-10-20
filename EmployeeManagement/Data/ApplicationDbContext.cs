using EmployeeManagement.Model;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    public DbSet<EmployeeDto> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeDto>()
            .HasKey(e => e.Id);

        // Add sample employees
        modelBuilder.Entity<EmployeeDto>().HasData(
            new EmployeeDto
            {
                Id = 1,
                FirstName = "Rahul",
                LastName = "Shukla",
                Email = "rs@gmail.com",
                Age = 30
            },
            new EmployeeDto
            {
                Id = 2,
                FirstName = "Raul",
                LastName = "Shukla",
                Email = "raul@gmail.com",
                Age = 25
            });

        // Make the combination of first name, last name and email address unique.
        modelBuilder.Entity<EmployeeDto>()
            .HasIndex(e => new { e.FirstName, e.LastName, e.Email, e.Age, e.Address })
            .IsUnique();
    }
}

