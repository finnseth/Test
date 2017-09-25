using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.PortalService.Controllers.Network.Setup.InternetGateway.Model
{
    public class CommunicationMethodModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string CarrierName { get; set; }
        public bool Enabled { get; set; }
    }
}
