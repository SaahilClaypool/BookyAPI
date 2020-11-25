using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookyApi.API.Auth;
using BookyApi.API.Db;
using BookyApi.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentValidation;
using BookyApi.API.Services.Extensions;

namespace BookyApi.API.Controllers
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
        public async Task<IEnumerable<Bookmark>> Index(
            [FromServices] User currentUser
        )
        {
            return await currentUser.BookmarkQuery(Context).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Bookmark> Get(
            [FromRoute] int id,
            [FromServices] User currentUser
        )
        {
            return await currentUser.BookmarkQuery(Context)
                .Where(b => b.Id == id)
                .FirstAsync();
        }


        [HttpPost]
        public async Task<int> Create(
            [FromBody] Bookmark bookmark,
            [FromServices] User currentUser
        )
        {
            bookmark.User = currentUser;
            new BookmarkValidator().ValidateAndThrow(bookmark);
            Context.Add(bookmark);
            await Context.SaveChangesAsync();
            return (int)bookmark.Id!;
        }
        public ILogger Logger { get; }
        public BookyContext Context { get; }
    }
}
