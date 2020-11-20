using System.Linq;
using BookyApi.Db;
using BookyApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookyApi.Services.Extensions
{
    public static class EfExtensions
    {
        public static string TableName<T>(this BookyContext context)
        {
            var entityType = context.Model.FindEntityType(typeof(T));
            return entityType.GetTableName();
        }

        public static IQueryable<Bookmark> BookmarkQuery(this User user, BookyContext context) =>
            context.Entry(user).Collection(user => user.Bookmarks).Query();
    }
}