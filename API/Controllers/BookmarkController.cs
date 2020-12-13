using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BookyApi.API.Auth;
using BookyApi.API.Db;
using BookyApi.API.Models;
using BookyApi.API.Services.Extensions;
using BookyApi.Shared.DTO;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookyApi.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<BookmarkDetailsDTO> Get(
            [FromRoute] int id,
            [FromServices] User currentUser
        )
        {
            var bookmark = await currentUser.BookmarkQuery(Context)
                .Where(b => b.Id == id)
                .FirstAsync();
            return bookmark switch {
                Bookmark mark => new BookmarkDetailsDTO { Id = mark.Id, Notes = mark.Notes, Url = mark.Url, Content = mark.Content },
                _ => new BookmarkDetailsDTO()
            };
        }

        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public async Task<bool> Update(
            [FromRoute] int id,
            [FromBody] BookmarkDetailsDTO bookmarkDetails,
            [FromServices] User currentUser
        )
        {

            Logger.LogInformation(bookmarkDetails.AsJson());
            var dbBookmark = await currentUser.BookmarkQuery(Context)
                .Where(b => b.Id == id)
                .FirstAsync();

            if(dbBookmark is null) return false;
            dbBookmark.Notes = bookmarkDetails.Notes;
            new BookmarkValidator().ValidateAndThrow(dbBookmark);
            await Context.SaveChangesAsync();
            // TODO: pull in notes
            return true;
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
