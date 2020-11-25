using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookyApi.API.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<Bookmark> Bookmarks { get; set; } = null!;
    }
}