using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dualog.PortalService.Controllers.Users.Model
{
    public class UserListDetails
    {
        public IEnumerable<UserSummaryModel> Users { get; set; }
        public int TotalCount { get; set; }
    }

    public class UserSummaryModel
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public bool IsVesselUser { get; set; }
        
    }
}