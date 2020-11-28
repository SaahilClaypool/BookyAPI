using System;

namespace BookyApi.Shared.DTO
{
    public class LoginDTO
    {
        public string Username { get; set; } = "unset";
        public string Password { get; set; } = "unset";
    }

    public class LoginResultDTO
    {
        public bool Success { get; set; } = false;
        public string Token { get; set; } = "unset";
    }

}
