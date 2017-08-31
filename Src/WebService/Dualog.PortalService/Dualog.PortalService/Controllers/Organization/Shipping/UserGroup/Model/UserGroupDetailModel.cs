using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model
{
    public class UserGroupDetailModel : UserGroupModel
    {

        [StringLength(300, ErrorMessage = "description cannot be longer than 300 characters.")]
        public string Description { get; set; }

        public IEnumerable<UserMemberModel> Members { get; set; }


    }
}
