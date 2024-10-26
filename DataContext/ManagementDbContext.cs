using Microsoft.EntityFrameworkCore;

public class ManagementDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Journal> Journals { get; set; }

    public ManagementDbContext(DbContextOptions<ManagementDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Journal>()
            .HasIndex(j => new { j.Date, j.EmployeeId })
            .IsUnique();
    }
}