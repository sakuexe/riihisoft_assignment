using Microsoft.EntityFrameworkCore;

using riihisoft.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new NotImplementedException("ConnectionString of 'DefaultConnection' was not found");

builder.Services.AddDbContext<CatbaseContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

await SeedDatabase.Initialize(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
User currentUser = new User("Sakuk");

app.MapGet("/user", () =>
{
    return currentUser;
})
.WithName("GetUser")
.WithOpenApi();

app.MapPut("/user", (string username) =>
{
    // reassign current user, because records are immutable
    currentUser = currentUser with { Username = username };
    return currentUser;
})
.WithName("UpdateUser")
.WithOpenApi();

app.MapGet("/cats", (CatbaseContext db) =>
{
    Cat[] cats = db.Cats.Include(c => c.Reviews).ToArray();
    return cats;
})
.WithName("GetCats")
.WithOpenApi();

app.MapGet("/reviews", (CatbaseContext db) =>
{
    CatReview[] reviews = db.CatReviews.ToArray();
    return reviews;
})
.WithName("GetCatReviews")
.WithOpenApi();

app.Run();

record User(string Username)
{
    public string Greeting => $"Hello there, {Username}";
    public DateTime createdAt = DateTime.Now;
}