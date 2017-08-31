using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Contact.Model
{
    public class UserAddressGroupModel : CompanyAddressGroupModel
    {

        public long User { get; set; }
        
        public static new UserAddressGroupModel FromDsAddressGroup(DsAddressGroup addressgroup)
        {
            var config = (UserAddressGroupModel) CompanyAddressGroupModel.FromDsAddressGroup(addressgroup);
            config.User = addressgroup.User.Id;

            return config;
        }
    }
}
