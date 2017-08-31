using System;
using System.Linq;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using Dualog.PortalService.Controllers.Organization.Admin.Model;
using Dualog.Data.Oracle.Entity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Dualog.PortalService.Core.Data;
using Microsoft.AspNetCore.Mvc;


namespace Dualog.PortalService.Controllers.Organization.Admin
{
    public class AdminUserRepository
    {

        IDataContextFactory _dcFactory;

        public AdminUserRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }


        public async Task<AdminUserDetailModel> GetDetailedUser(long id)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from c in dc.GetSet<DsAdminUser>()
                         where c.Id == id 
                         select new AdminUserDetailModel
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Address = c.Address,
                             Phone = c.PhoneNumber,
                             Email = c.Email,
                             Created = c.CreatedAt
                         };

                var adminuser = await qc.FirstOrDefaultAsync();

                return adminuser;
            }
        }

        public async Task<IEnumerable <AdminUserModel>> GetUser()
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from c in dc.GetSet<DsAdminUser>()
                         select new AdminUserDetailModel
                         {
                             Id = c.Id,
                             Name = c.Name,
                         };

                var adminuser = await qc.ToListAsync();

                return adminuser;
            }
        }

    }
}
