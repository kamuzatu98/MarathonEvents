using Microsoft.EntityFrameworkCore;
using MarathonEvents.Models;

namespace MarathonEvents.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<MarathonEvents.Models.Route> Routes { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;
    public DbSet<MarathonEvents.Models.Results> Results { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // No explicit relationships configured: models currently lack
        // consistent navigation properties. Rely on EF Core conventions.
    }
}


  
