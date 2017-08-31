using System;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Dualog.PortalService.Controllers.Permissions;

namespace Dualog.PortalService.Core
{
    public class ResourcePermissionFilter : IAsyncActionFilter
    {
        IDataContextFactory _dcFactory;
        string _resourceName;
        AccessRights _access;

        public ResourcePermissionFilter(string resourceName, AccessRights access, IDataContextFactory dcFactory)
        {
            _access = access;
            _resourceName = resourceName;
            _dcFactory = dcFactory;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub");
            if (userClaim == null || long.TryParse(userClaim.Value, out var userId) == false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var isAdmin = context.HttpContext.User.Claims.Any(c => c.Type == "xdadmin");


            var provider = new PermissionRepository(_dcFactory);
            var hasAccess = await provider.CheckPermission(userId, _resourceName, _access, isAdmin);
            if( hasAccess )
                await next();
            else
                context.Result = new UnauthorizedResult();
        }
    }
}
