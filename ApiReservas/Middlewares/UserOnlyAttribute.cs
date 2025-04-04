﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiReservas.Middlewares
{
    public class UserOnlyAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAdminClaim = context.HttpContext.User.Claims.FirstOrDefault(u => u.Type == "IsAdmin")?.Value;

            if (isAdminClaim == null || isAdminClaim.ToLower() == "true")
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
