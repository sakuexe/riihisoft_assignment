using Microsoft.EntityFrameworkCore;
using riihisoft.Database;
using riihisoft.Models;

namespace riihisoft.Handlers;

public static class ReviewsHandler
{
    public static IResult GetReviews(HttpContext context, CatbaseContext db)
    {
        string? limitRaw = context.Request.Query["limit"];
        int limit = 0; // default the limit to 0 (all results)

        // validate the limit query if there is one
        if (limitRaw != null && !int.TryParse(limitRaw, out limit))
            return Results.BadRequest("Invalid limit value. The value must be a positive integer");
        if (limit < 0) {
            return Results.BadRequest("Invalid limit value. The value must be a positive integer");
        }

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
    }

    public static async Task<IResult> AddReview(HttpContext context, CatbaseContext db)
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

        CatReview newReview = new()
        {
            Title = form["title"].ToString().Trim(),
            Description = form["review"].ToString().Trim(),
            Rating = rating,

            CatId = chosenCat.CatId,
            Cat = chosenCat,
        };
        await db.CatReviews.AddAsync(newReview);
        await db.SaveChangesAsync();

        return Results.Ok();
    }
}