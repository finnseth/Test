using Dualog.PortalService.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Ship.Model
{
    public class AdminShipModel : ShipModel
    {

        public long? Billing { get; set; }

        public DateTime? TimeAdded { get; set; }

        public ShipCategory? Category { get; set; }

        public long DualogUnit { get; set; }
    }
}
