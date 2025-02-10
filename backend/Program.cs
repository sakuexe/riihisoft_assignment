using Microsoft.EntityFrameworkCore;

using riihisoft.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
    });
});

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new NotImplementedException("ConnectionString of 'DefaultConnection' was not found");

builder.Services.AddDbContext<CatbaseContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

await SeedDatabase.Initialize(app);

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/cats", (CatbaseContext db) =>
{
    CatDto[] cats = db.Cats.Select(c => CatDto.ToDto(c)).ToArray();
    return Results.Ok(cats);
})
.WithName("GetCats")
.WithOpenApi();

app.MapGet("/reviews", (CatbaseContext db, HttpContext context) =>
{
    string? limitRaw = context.Request.Query["limit"];
    int limit = 0; // default the limit to 0 (all results)

    // validate the limit query if there is one
    if (limitRaw != null && !int.TryParse(limitRaw, out limit))
        return Results.BadRequest("Invalid limit value. The value must be a positive integer");
    if (limit < 0)
        return Results.BadRequest("Invalid limit value. The value must be a positive integer");

    CatReviewDto[] reviews;
    if (limit > 0)
        reviews = db.CatReviews
            .OrderByDescending(r => r.CatReviewId)
            .Take(limit)
            .Include(r => r.Cat)
            .Select(r => CatReviewDto.ToDto(r))
            .ToArray();
    else
        reviews = db.CatReviews
            .OrderByDescending(r => r.CatReviewId)
            .Include(r => r.Cat)
            .Select(r => CatReviewDto.ToDto(r))
            .ToArray();

    return Results.Ok(reviews);
})
.WithName("GetCatReviews")
.WithOpenApi();

app.MapPost("/reviews", async (CatbaseContext db, HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    int catId;
    int rating;

    // validate the request
    if (!int.TryParse(form["cat-id"], out catId))
        return Results.BadRequest("field 'cat-id' was not found within request or was not a valid integer");
    if (!int.TryParse(form["rating"], out rating))
        return Results.BadRequest("field 'rating' was not found within request or was not a valid integer");
    if (!form.ContainsKey("title"))
        return Results.BadRequest("field 'title' was not found within request");
    if (form["title"].ToString().Trim() == "")
        return Results.BadRequest("field 'title' cannot be empty");
    if (!form.ContainsKey("review"))
        return Results.BadRequest("field 'review' was not found within request");
    if (form["review"].ToString().Trim() == "")
        return Results.BadRequest("field 'review' cannot be empty");

    // get the cat
    Cat? chosenCat = await db.Cats.FirstOrDefaultAsync(c => c.CatId == catId);
    if (chosenCat == null)
        return Results.UnprocessableEntity($"Cat with id {catId} not found in the database");

    CatReview newReview = new() {
        Title = form["title"].ToString().Trim(),
        Description = form["review"].ToString().Trim(),
        Rating = rating,

        CatId = chosenCat.CatId,
        Cat = chosenCat,
    };
    await db.CatReviews.AddAsync(newReview);
    await db.SaveChangesAsync();

    return Results.Ok();
})
.WithName("AddReview")
.WithOpenApi();

app.Run();

// use data transfer objects, because I wanted to include the cat information
// in the review query, without creating an infinite recursion problem
record CatDto(int CatId, string Name, string? ImageUrl)
{
    public static CatDto ToDto(Cat cat) {
        return new CatDto(cat.CatId, cat.Name, cat.ImageUrl);
    }
}

record CatReviewDto(int CatReviewId, string Title, string Description, int Rating, string? CreatedAt, CatDto Cat)
{
    public static CatReviewDto ToDto(CatReview review)
    {
        return new CatReviewDto(
            review.CatReviewId,
            review.Title,
            review.Description,
            review.Rating,
            review.CreatedAt,
            CatDto.ToDto(review.Cat)
        );
    }
}