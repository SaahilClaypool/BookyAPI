using System;
using System.Text.Json.Serialization;

using FluentValidation;

using Microsoft.Extensions.Logging;

namespace BookyApi.API.Models
{
    public abstract class Entity
    {
        public int? Id { get; set; }
    }
    public class Bookmark : Entity
    {
        public static readonly int MaxLength = 280;
        public string Url { get; set; } = null!;
        public string? Content { get; set; }
        public string? Notes { get; set; }
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
            RuleFor(b => b.Notes).MaximumLength(Bookmark.MaxLength);
        }
    }
}
