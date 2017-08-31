using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Dualog.PortalService.Core
{
    public class ResourcePermissionAttribute : TypeFilterAttribute
    {
        public ResourcePermissionAttribute(string resourceName, AccessRights access) :
            base(typeof(ResourcePermissionFilter))
        {
            Arguments = new object[] { resourceName, access };
        }
    }
}
