using Microsoft.EntityFrameworkCore;
using PolyplayAPI.Models.Auth;
using PolyplayAPI.Models.Logging;

namespace PolyplayAPI.Models;

public class PolyplayDbContext(DbContextOptions<PolyplayDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<GameComment> GameComments { get; set; } = null!;

    public DbSet<UserActivity> UserActivityLog { get; set; } = null!;

    public DbSet<ActivityType> ActivityTypes { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;

    public DbSet<UsersRole> UsersRoles { get; set; } = null!;
    public DbSet<RolesPermission> RolesPermissions { get; set; } = null!;

    public DbSet<Role> Roles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // to map self-referencing many-to-many relationship
        modelBuilder.Entity<User>()
            .HasMany(e => e.Friends)
            .WithMany();

    }

}
