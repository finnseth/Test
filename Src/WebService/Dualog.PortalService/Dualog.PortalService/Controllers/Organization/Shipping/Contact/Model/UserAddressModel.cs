using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Contact.Model
{
    public class UserAddressModel : CompanyAddressModel
    {

        public long User { get; set; }
        
        public static new UserAddressModel FromDsAddress(DsAddress address)
        {
            var config = (UserAddressModel) CompanyAddressModel.FromDsAddress(address);
            config.User = address.User.Id;

            return config;
        }
    }
}
