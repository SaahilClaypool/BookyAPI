using System;

namespace BookyApi.API.Errors
{
    public class ApplicationError : Exception
    {
        public ApplicationError(string? message) : base(message)
        {
        }
    }
}