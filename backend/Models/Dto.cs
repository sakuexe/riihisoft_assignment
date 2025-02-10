using riihisoft.Database;

namespace riihisoft.Models;

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