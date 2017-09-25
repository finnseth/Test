using System;
using System.Linq;

namespace Dualog.PortalService.Controllers.Network.BandwidthRestriction.Model
{
    public class VesselRule : CompanyRule
    {
        public bool IsActive { get; set; }
        public bool IsCompanyRule { get; set; }
    }
}
