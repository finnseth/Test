using System;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dualog.PortalService.Core
{
    public class IsInCompanyFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get the Company Id as a string
            var claimsIdentity = context.HttpContext.User.Identities.First();
            var sCompanyId = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "xdcid")?.Value;

            if (sCompanyId == null)
                sCompanyId = context.HttpContext.Request.Headers["xdcid"];


            // Convert the string to a long
            long cid;

            if (long.TryParse(sCompanyId, out cid))
                await next();
            else
                context.Result = new UnauthorizedResult();

        }
    }
}
