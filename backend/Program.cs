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
    Cat[] cats = db.Cats.Include(c => c.Reviews).ToArray();
    return cats;
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

    CatReview[] reviews;
    if (limit > 0)
        reviews = db.CatReviews.OrderByDescending(r => r.CatReviewId).Take(limit).ToArray();
    else
        reviews = db.CatReviews.ToArray();

    return Results.Ok(reviews);
})
.WithName("GetCatReviews")
.WithOpenApi();

app.Run();