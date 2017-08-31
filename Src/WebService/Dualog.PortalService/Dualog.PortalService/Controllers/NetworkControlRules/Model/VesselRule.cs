using System;
using System.Linq;

namespace Dualog.PortalService.Controllers.NetworkControlRules.Model
{
    public class VesselRule : CompanyRule
    {
        public bool IsActive { get; set; }
        public bool IsCompanyRule { get; set; }
    }
}
