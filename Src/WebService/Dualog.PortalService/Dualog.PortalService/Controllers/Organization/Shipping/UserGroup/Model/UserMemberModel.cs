using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;


namespace Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model
{
    public class UserMemberModel : DataAnnotationsValidatable
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

    }
}
