namespace riihisoft.Models;

public class Cat
{
    public int CatId { get; set; }
    public string Name { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public int NumberOfReviews => Reviews.Count;

    public List<CatReview> Reviews { get; } = new();
}

record CatDto(int CatId, string Name, string? ImageUrl)
{
    public static CatDto ToDto(Cat cat) {
        return new CatDto(cat.CatId, cat.Name, cat.ImageUrl);
    }
}