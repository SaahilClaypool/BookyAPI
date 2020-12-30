using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using BookyApi.API.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookyApi.API.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public List<string> Roles { get; set; }
        public JwtAuthorizeAttribute() : this(new List<string>()) { }
        public JwtAuthorizeAttribute(List<string> roles)
        {
            Roles = roles;
        }
        public JwtAuthorizeAttribute(string role) : this(new List<string>() { role }) { }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            if (Roles.Count != 0)
            {
                if (!user.Claims.Where(c => c.Type == ClaimTypes.Role && Roles.Contains(c.Value)).Any())
                {
                    context.Result = new JsonResult(new { message = $"Not in role: {string.Join(",", Roles)}" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
        }
    }
}
