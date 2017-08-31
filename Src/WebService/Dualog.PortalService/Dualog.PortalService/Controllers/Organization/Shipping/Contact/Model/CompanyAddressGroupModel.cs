using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Contact.Model
{
    public class CompanyAddressGroupModel : AddressGroupModel
    {

        public long Company { get; set; }

        public static new CompanyAddressGroupModel FromDsAddressGroup(DsAddressGroup addressgroup)
        {
            var config = (CompanyAddressGroupModel)AddressGroupModel.FromDsAddressGroup(addressgroup);
            config.Company = addressgroup.Company.Id;

            return config;
        }
    }
}
