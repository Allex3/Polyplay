using Microsoft.EntityFrameworkCore;

namespace PolyplayAPI.Models;

public class PolyplayDbContext(DbContextOptions<PolyplayDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<GameComment> GameComments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // to map self-referencing many-to-many relationship
        modelBuilder.Entity<User>()
            .HasMany(e => e.Friends)
            .WithMany();
    }

}
