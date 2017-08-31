using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.PortalService.Controllers.Methods.Model
{
    public class CarrierTypeDetails
    {
        public long Id { get; set; }
        public int ModemHandShake { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
    }
}
