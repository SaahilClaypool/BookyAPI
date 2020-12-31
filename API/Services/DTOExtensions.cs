using BookyApi.API.Models;
using BookyApi.Shared.DTO;

using System.Linq;

namespace BookyApi.API.Services.Extensions
{
    public class ConvertDTO
    {
        public static BookmarkDetailsDTO From(Bookmark bookmark) =>
            new()
            {
                Content = bookmark.Content,
                Id = bookmark.Id,
                Notes = bookmark?.Notes?.Select(To).ToList(),
                Url = bookmark?.Url
            };

        public static Bookmark From(BookmarkDetailsDTO bookmark) =>
            new()
            {
                Content = bookmark.Content,
                Id = bookmark.Id,
                Notes = bookmark.Notes.Select(From).ToList(),
                Url = bookmark.Url
            };

        public static NoteDTO To(Note note) =>
            new()
            {
                Id = note.Id!.Value,
                Comment = note.Comment,
                TextFragment = note.TextFragment
            };

        public static Note From(NoteDTO note) =>
            new()
            {
                Id = note.Id,
                Comment = note.Comment,
                TextFragment = note.TextFragment
            };
    }
}