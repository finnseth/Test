using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Contact.Model
{
    public class CompanyAddressModel : AddressModel
    {

        public long Company { get; set; }

        public static new CompanyAddressModel FromDsAddress(DsAddress address)
        {
            var config = (CompanyAddressModel) AddressModel.FromDsAddress(address);
            config.Company = address.Company.Id;

            return config;
        }
    }
}
