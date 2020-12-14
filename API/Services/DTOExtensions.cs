using BookyApi.API.Models;
using BookyApi.Shared.DTO;

namespace BookyApi.API.Services.Extensions
{
    public class ConvertDTO
    {
        public static BookmarkDetailsDTO From(Bookmark bookmark) =>
            new()
            {
                Content = bookmark.Content,
                Id = bookmark.Id,
                Notes = bookmark.Notes,
                Url = bookmark.Url
            };

        public static Bookmark From(BookmarkDetailsDTO bookmark) =>
            new()
            {
                Content = bookmark.Content,
                Id = bookmark.Id,
                Notes = bookmark.Notes,
                Url = bookmark.Url
            };
    }
}