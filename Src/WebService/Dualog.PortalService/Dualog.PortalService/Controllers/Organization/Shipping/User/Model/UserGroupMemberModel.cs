using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;


namespace Dualog.PortalService.Controllers.Organization.Shipping.User.Model
{
    public class UserGroupMemberModel : DataAnnotationsValidatable
    {
        public long Id { get; set; }

        public long UserGroupId { get; set; }

        public string UserGroupName { get; set; }

    }
}
