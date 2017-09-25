using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.PortalService.Controllers.Network.BandwidthRestriction.Model
{
    public class CompanyRule
    {
        public long Id { get; set; }
        public Method Method { get; set; }
        public string Description { get; set; }
        public string SourceComputer { get; set; }
        public short Priority { get; set; }
        public Service Service { get; set; }

        public IEnumerable<string> Destinations { get; set; }
    }
}
