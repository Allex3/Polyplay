using Microsoft.EntityFrameworkCore;

namespace PolyplayAPI.Models;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; } = null!;
}
