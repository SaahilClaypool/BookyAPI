using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookyApi.Auth;
using BookyApi.Db;
using BookyApi.Models;
using BookyApi.Services.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookyApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        public SearchController(ILogger<SearchController> logger, BookyContext context)
        {
            Logger = logger;
            Context = context;
        }

        [HttpGet("{query}")]
        public async Task<IEnumerable<Bookmark>> Search(
            [FromRoute] string query,
            [FromServices] User currentUser
        )
        {
            var table = Context.TableName<Bookmark>();
            Logger.LogCritical(table);
            // TODO find way to put into postgres function
            return await Context.Bookmarks
                .FromSqlRaw(@$"
                    SELECT *, LENGTH(Content) - LENGTH(REPLACE(Content, '{query}', '')) as occurences FROM {table} as t
                    where instr(Content, '{query}') > 0
                    order by occurences desc
                ")
                .Where(b => b.UserId == currentUser.Id)
                .ToListAsync();
        }

        public ILogger Logger { get; }
        public BookyContext Context { get; }
    }
}
