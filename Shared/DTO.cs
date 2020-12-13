using System;
using System.Collections.Generic;

namespace BookyApi.Shared.DTO
{
    public class SearchResultDTO
    {
        public List<BookmarkSummaryDTO> Result { get; set; } = new();
    }

    public class BookmarkSummaryDTO
    {
        public string Url { get; set; } = null!;
        public string? Content { get; set; }
        public int? Id { get; set; }

    }

    public class BookmarkDetailsDTO
    {
        public string Url { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Notes { get; set; } = null!;
        public int? Id { get; set; }
    }

    public class LoginDTO
    {
        public string Username { get; set; } = "unset";
        public string Password { get; set; } = "unset";
    }

    public class LoginResultDTO
    {
        public bool Success { get; set; } = false;
        public string Token { get; set; } = "unset";
        public string UserName { get; set; } = "unset";
    }

}
