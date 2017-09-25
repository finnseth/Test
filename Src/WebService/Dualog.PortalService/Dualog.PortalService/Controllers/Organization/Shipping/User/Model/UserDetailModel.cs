using System;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Permission.Model;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Controllers.Organization.Shipping.User.Model
{
    public class UserDetailModel : UserModel
    {

        [StringLength( 20, MinimumLength = 1, ErrorMessage = "Phone number must be between 1 and 20 characters long.")]
        public string PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }

        public IEnumerable<UserGroupModel> Usergroup { get; set; }
        public IEnumerable<PermissionDetailModel> Permissions { get; set; }

        public static UserDetailModel FromDsUser(DsUser user)
        {
            return new UserDetailModel
            {
                Address = user.Address,
                Email = user.Email,
                Id = user.Id,
                IsVesselUser = user.VesselUser ?? false,
                Name = user.Name,
                PhoneNumber = user.PhoneNr,
                Permissions = from p in user.Permissions
                              select new PermissionDetailModel
                              {
                                  Name = p.Function.Name,
                                  AllowType = (AccessRights)p.AllowType
                              },
                Usergroup = from ug in user.UserGroups
                             select new UserGroupModel
                             {
                                 Name = ug.Name,
                                 Id = ug.Id
                             }
            };
        }


    }
}