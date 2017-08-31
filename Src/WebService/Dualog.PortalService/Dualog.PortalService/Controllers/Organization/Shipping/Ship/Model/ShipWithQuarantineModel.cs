using Dualog.PortalService.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Ship.Model
{
    public class ShipWithQuarantineModel : ShipModel
    {

        public bool QuarantineLocalChanges{ get; set; }

    }
}
