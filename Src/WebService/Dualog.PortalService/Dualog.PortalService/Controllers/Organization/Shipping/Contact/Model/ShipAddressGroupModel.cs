using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Contact.Model
{
    public class ShipAddressGroupModel : CompanyAddressGroupModel
    {

        public long Ship { get; set; }

        public static new ShipAddressGroupModel FromDsAddressGroup(DsAddressGroup addressgroup)
        {
            var config = (ShipAddressGroupModel) CompanyAddressGroupModel.FromDsAddressGroup(addressgroup);
            config.Ship = addressgroup.Vessel.Id;

            return config;
        }
    }
}
