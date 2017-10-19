using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Dualog.PortalService.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IsInDualogAttribute : TypeFilterAttribute
    {
        public IsInDualogAttribute() :
            base(typeof(IsInDualogFilter))
        {
        }
    }
}
