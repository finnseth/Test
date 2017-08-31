using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Contact.Model
{
    public class ShipAddressModel : CompanyAddressModel
    {

        public long Ship { get; set; }

        public static new ShipAddressModel FromDsAddress(DsAddress address)
        {
            var config = (ShipAddressModel) CompanyAddressModel.FromDsAddress(address);
            config.Ship = address.Vessel.Id;

            return config;
        }
    }
}
