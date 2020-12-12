using System;
using System.Linq;

using BookyApi.API.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookyApi.API.Db
{
    public class BookyContext : IdentityDbContext<User>
    {
        public BookyContext(DbContextOptions<BookyContext> options) : base(options)
        {
        }

        public DbSet<Bookmark> Bookmarks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookmarks)
                .WithOne(b => b.User!)
                .HasForeignKey(b => b.UserId!);
        }
    }
}
