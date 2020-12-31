using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BookyApi.API.Auth;
using BookyApi.API.Db;
using BookyApi.API.Models;
using BookyApi.API.Services.Extensions;
using BookyApi.API.Services.UseCases;
using BookyApi.Shared.DTO;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookyApi.API.Controllers
{
    [JwtAuthorize()]
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
        public async Task<IEnumerable<Bookmark>> Index()
        {
            return await User.BookmarkQuery(Context).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<BookmarkDetailsDTO> Get(
            [FromRoute] int id
        )
        {
            var bookmark = await User.BookmarkQuery(Context)
                .Where(b => b.Id == id)
                .Include(b => b.Notes)
                .FirstAsync();
            return bookmark switch
            {
                Bookmark mark => 
                    new BookmarkDetailsDTO { 
                        Id = mark.Id, 
                        Notes = mark.Notes?.Select(n => ConvertDTO.To(n)).ToList(), 
                        Url = mark.Url, 
                        Content = mark.Content 
                    },
                _ => new BookmarkDetailsDTO()
            };
        }

        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public async Task<bool> Update(
            [FromRoute] int id,
            [FromBody] BookmarkDetailsDTO bookmarkDetails
        )
        {

            Logger.LogInformation(bookmarkDetails.AsJson());
            var dbBookmark = await User.BookmarkQuery(Context)
                .Where(b => b.Id == id)
                .Include(b => b.Notes)
                .FirstAsync();

            if (dbBookmark is null) return false;
            dbBookmark.Notes = bookmarkDetails.Notes
                .Where(n => n.Comment.Trim().Length > 0) // HACK to ignore notes from the front end that are empty
                .Select(ConvertDTO.From)
                .ToList();
            Logger.LogInformation($"NOTE IDS: {string.Join(",", dbBookmark.Notes.Select(n => n.Id))}");
            new BookmarkValidator().ValidateAndThrow(dbBookmark);
            await Context.SaveChangesAsync();
            // TODO: pull in notes
            return true;
        }


        [HttpGet("Populate")]
        public async Task<BookmarkDetailsDTO> Populate(
            [FromQuery] string clipboardContents,
            [FromServices] PopulateFromClipboard service
        )
        {
            Logger.LogInformation($"Populating from clipboard contents: {clipboardContents}");
            var bookmark = await service.Populate(clipboardContents);
            return ConvertDTO.From(bookmark);
        }

        [HttpPost]
        public async Task<int> Create(
            [FromBody] BookmarkDetailsDTO details
        )
        {
            var bookmark = new Bookmark()
            {
                Content = details.Content,
                Notes = details.Notes.Select(ConvertDTO.From).ToList(),
                Url = details.Url,
                UserId = User.Identity?.Name
            };
            Logger.LogInformation(bookmark.AsJson());
            new BookmarkValidator().ValidateAndThrow(bookmark);
            Context.Add(bookmark);
            await Context.SaveChangesAsync();
            return (int)bookmark.Id!;
        }
        public ILogger Logger { get; }
        public BookyContext Context { get; }
    }
}
