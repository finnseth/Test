using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dualog.PortalService.Core
{
    public class IsInDualogFilter: IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get the Company Id as a string
            var claimsIdentity = context.HttpContext.User.Identities.First();
            var isDualogAdmin = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "xdadmin")?.Value;

            
            // Convert the string to a long
            Boolean isAdmin;

            if (Boolean.TryParse(isDualogAdmin, out isAdmin))
                await next();
            else
                context.Result = new UnauthorizedResult();

        }
    }
}
