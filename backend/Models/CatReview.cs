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