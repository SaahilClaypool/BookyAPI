namespace BookyApi.Models
{
    public abstract class Entity
    {
        public int? Id { get; set; }
    }
    public class Bookmark : Entity
    {
        public string Url { get; set; } = null!;
        public string? Content { get; set; }
    }
}