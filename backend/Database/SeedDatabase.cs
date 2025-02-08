using Microsoft.EntityFrameworkCore;

namespace riihisoft.Database;

public class SeedDatabase
{
    private static CatbaseContext _context = default!;

    public async static Task Initialize(IHost host)
    {
        using var scope = host.Services.CreateScope();
        {
            var services = scope.ServiceProvider;
            _context = services.GetRequiredService<CatbaseContext>();
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations != null)
                await _context.Database.MigrateAsync();
        }

        await SeedCats();
        await SeedReviews();
    }

    private async static Task SeedCats()
    {
        if (_context.Cats.Any())
            return;

        Cat[] cats = new[]
        {
            new Cat
            {
                Name = "Uni",
                ImageUrl = "/images/cats/uni.jpg",
            },
            new Cat
            {
                Name = "Goof",
                ImageUrl = "/images/cats/goof.jpg",
            },
            new Cat
            {
                Name = "Milly",
                ImageUrl = "/images/cats/silly.jpg",
            },
            new Cat
            {
                Name = "Oye",
                ImageUrl = "/images/cats/oye.jpg",
            },
        };

        await _context.Cats.AddRangeAsync(cats);
        await _context.SaveChangesAsync();
    }

    private async static Task SeedReviews()
    {
        if (_context.CatReviews.Any())
            return;

        Cat uni = _context.Cats.First(c => c.CatId == 1);
        Cat goof = _context.Cats.First(c => c.CatId == 2);
        Cat milly = _context.Cats.First(c => c.CatId == 3);
        Cat oye = _context.Cats.First(c => c.CatId == 4);

        uni.Reviews.Add(GenerateReview("He was the strongest", "He was always trying to fight me... What might be going on behind those eyes if his?", 8));
        uni.Reviews.Add(GenerateReview("What a baby", "He ate a whole rotisserie chicken in one gulp! Wow!", 7));
        goof.Reviews.Add(GenerateReview("Kinda scary", "I am pretty sure that that's a real man trapped in a cat's physique...", 3));
        milly.Reviews.Add(GenerateReview("What a silly billy", "That might be the silliest cat I have ever seen.", 10));
        oye.Reviews.Add(GenerateReview("Woah, very powerful", "He just broke through my wall, I did not even know of this service before.", 10));

        await _context.SaveChangesAsync();
    }

    private static CatReview GenerateReview(string title, string description, int rating)
    {
        return new CatReview
        {
            Title = title,
            Description = description,
            Rating = rating,
        };
    }
}