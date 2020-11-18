using System.Linq;
using BookyApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookyApi.Db
{
    public class BookyContext : IdentityDbContext<User>
    {
        public DbSet<Bookmark> Bookmarks { get; set; } = null!;

        public BookyContext(DbContextOptions<BookyContext> options) : base(options)
        {
        }
    }
}