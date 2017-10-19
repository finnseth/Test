using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Dualog.PortalService.Core
{
    public class IsInCompanyAttribute : TypeFilterAttribute
    {
        public IsInCompanyAttribute() :
            base(typeof(IsInCompanyFilter))
        {
        }
    }
}
