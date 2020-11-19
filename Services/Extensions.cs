using BookyApi.Db;
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
    }
}