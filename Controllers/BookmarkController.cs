using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookyApi.Auth;
using BookyApi.Db;
using BookyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookyApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BookmarkController : ControllerBase
    {
        public BookmarkController(ILogger<BookmarkController> logger, BookyContext context)
        {
            Logger = logger;
            Context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Bookmark>> Index()
        {
            return await Context.Bookmarks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Bookmark> Get(
            [FromRoute] int id
        )
        {
            return await Context.Bookmarks.FindAsync(id);
        }


        [HttpPost]
        public async Task<int> Create(
            [FromBody] Bookmark bookmark
        )
        {
            Context.Bookmarks.Add(bookmark);
            await Context.SaveChangesAsync();
            return (int)bookmark.Id!;
        }
        public ILogger Logger { get; }
        public BookyContext Context { get; }
    }
}
