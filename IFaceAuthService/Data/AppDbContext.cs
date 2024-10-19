using IFaceAuthService.Entities;
using Microsoft.EntityFrameworkCore;

namespace IFaceAuthService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(e => e.Id);
            e.HasIndex(e => e.Email).IsUnique();
            e.HasQueryFilter(x => !x.Deleted);
        });
    }
}
