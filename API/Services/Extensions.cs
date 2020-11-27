using System;
using System.Linq;
using System.Text.Json;
using BookyApi.API.Db;
using BookyApi.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookyApi.API.Services.Extensions
{
    public static class JsonExtensions
    {
        public static void Dbg(this object o) =>
            Console.WriteLine(JsonSerializer.Serialize(o, new()
            {
                WriteIndented = true
            }));
    }
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