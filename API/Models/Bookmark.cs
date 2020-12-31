using System;
using System.Collections.Generic;
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
        public string Url { get; set; } = null!;
        // full ish content of the bookmark
        public string? Content { get; set; }
        public string? UserId { get; set; } = null!;
        [JsonIgnore]
        public User? User { get; set; } = null!;

        public ICollection<Note>? Notes { get; set; }
    }

    public class Note : Entity
    {
        public static readonly int MaxLength = 280;
        public int BookmarkId { get; set; }
        [JsonIgnore]
        public Bookmark? Bookmark { get; set; }

        public string? TextFragment { get; set; }
        public string? Comment { get; set; }
    }

    public class NoteValidator : AbstractValidator<Note>
    {
        public NoteValidator()
        {
            RuleFor(n => n.Comment).MaximumLength(Note.MaxLength);
            RuleFor(n => n.TextFragment).MaximumLength(Note.MaxLength);
        }
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
