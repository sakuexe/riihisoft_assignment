using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace riihisoft.Database;

public class Cat
{
    public int CatId { get; set; }
    public string Name { get; set; } = default!;
    public string? ImageUrl { get; set; }

    public List<CatReview> Reviews { get; } = new();
}

public class CatReview
{
    public int CatReviewId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    [Range(1, 10)]
    public int Rating { get; set; } = default!;
    public readonly string CreatedAt = DateTime.Now.ToLongDateString();

    public int CatId { get; set; }
    [JsonIgnore]
    public Cat Cat { get; set; } = default!;
}