using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Permission.Model;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model
{
    public class UserGroupDetailModel : UserGroupModel
    {

        [StringLength(300, ErrorMessage = "description cannot be longer than 300 characters.")]
        public string Description { get; set; }

        public IEnumerable<UserMemberModel> Members { get; set; }

        public IEnumerable<PermissionDetailModel> Permissions { get; set; }

        public static UserGroupDetailModel FromDsUSerGroup(DsUserGroup userGroup)
        {
            var newUserGroupDetails = new UserGroupDetailModel()
            {
                Description = userGroup.Description,
                Id = userGroup.Id,
                Name = userGroup.Name,
                Permissions = userGroup.Permissions
                                       .Where(p => p.AllowType > 0)
                                       .Select(p => new PermissionDetailModel
                                       {
                                           AllowType = (AccessRights)p.AllowType,
                                           Name = p.Function.Name
                                       })
            };
            return newUserGroupDetails;
        }

    }
}
