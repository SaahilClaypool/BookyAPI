using System;
using System.Text.Json.Serialization;
using FluentValidation;

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
        public string? UserId { get; set; } = null!;
        [JsonIgnore]
        public User? User { get; set; } = null!;
    }

    public class BookmarkValidator : AbstractValidator<Bookmark>
    {
        public BookmarkValidator()
        {
            RuleFor(b => b.Url).Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
                .WithMessage("Must be a valid URL");
            RuleFor(b => b.Content).NotNull();
        }
    }
}