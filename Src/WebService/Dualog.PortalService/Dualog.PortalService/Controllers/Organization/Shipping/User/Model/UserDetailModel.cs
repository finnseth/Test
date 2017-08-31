using System;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;




namespace Dualog.PortalService.Controllers.Organization.Shipping.User.Model
{
    public class UserDetailModel : UserModel
    {

        [StringLength( 20, MinimumLength = 1, ErrorMessage = "Phone number must be between 1 and 20 characters long.")]
        public string PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }

        public IEnumerable<UserGroupMemberModel> Membership { get; set; }


    }
}