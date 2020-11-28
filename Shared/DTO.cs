using System;

namespace BookyApi.Shared.DTO
{
    public class LoginDTO
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
    public record LoginResultDTO(bool Success, string Token);
}
