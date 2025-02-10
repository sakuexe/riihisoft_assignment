using Microsoft.EntityFrameworkCore;
using riihisoft.Database;
using riihisoft.Handlers;
using riihisoft.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
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

app.MapGet("/reviews", ReviewsHandler.GetReviews)
.WithName("GetReviews")
.WithOpenApi();

app.MapPost("/reviews", ReviewsHandler.AddReview)
.WithName("AddReview")
.WithOpenApi();

app.Run();