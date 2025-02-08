using Microsoft.EntityFrameworkCore;

namespace riihisoft.Database;

public class CatbaseContext : DbContext
{
    public DbSet<Cat> Cats { get; set; } = default!;
    public DbSet<CatReview> CatReviews { get; set; } = default!;

    public CatbaseContext(DbContextOptions<CatbaseContext> options) : base(options)
    {
    }
}