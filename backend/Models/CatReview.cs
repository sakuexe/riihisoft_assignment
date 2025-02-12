using System.ComponentModel.DataAnnotations;

namespace riihisoft.Models;

public class CatReview
{
    public int CatReviewId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    [Range(1, 5)]
    public int Rating { get; set; } = default!;
    public readonly string CreatedAt = DateTime.Now.ToLongDateString();

    public int CatId { get; set; }
    public Cat Cat { get; set; } = default!;
}

// use data transfer objects, because I wanted to include the cat information
// in the review query, without creating an infinite recursion problem
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