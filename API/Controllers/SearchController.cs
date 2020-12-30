using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using BookyApi.API.Auth;
using BookyApi.API.Db;
using BookyApi.API.Models;
using BookyApi.API.Services.Extensions;
using BookyApi.Shared.DTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookyApi.API.Controllers
{
    [JwtAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        public SearchController(ILogger<SearchController> logger, BookyContext context)
        {
            Logger = logger;
            Context = context;
        }

        [HttpGet("{query}")]
        public async Task<SearchResultDTO> Search(
            [FromRoute] string query
        )
        {
            query = System.Web.HttpUtility.UrlDecode(query);
            var table = Context.TableName<Bookmark>();
            Logger.LogCritical(table);
            // TODO find way to put into postgres function
            var searchResult = await Context.Bookmarks
                .FromSqlInterpolated(@$"
                    SELECT ""Bookmarks"".* from search_bookmarks({query})
                    INNER JOIN ""Bookmarks""
                    ON ""Bookmarks"".""Id"" = id
                ")
                .Where(b => b.UserId == User.Identity!.Name)
                .ToListAsync();

            return new()
            {
                Result = searchResult.Select(b => new BookmarkSummaryDTO() { Content = b.Content, Url = b.Url, Id = b.Id }).ToList()
            };
        }

        public ILogger Logger { get; }
        public BookyContext Context { get; }
    }
}
