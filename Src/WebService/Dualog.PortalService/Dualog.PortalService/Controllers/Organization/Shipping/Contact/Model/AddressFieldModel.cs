using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Contact.Model
{
    public class AddressFieldModel
    {
        public long Id { get; set; }

        public long Field { get; set; }

        public string Value { get; set; }
    }
}
