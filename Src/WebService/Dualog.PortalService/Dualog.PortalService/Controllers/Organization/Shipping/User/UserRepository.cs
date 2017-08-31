using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.User.Model;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Data.Entity;

namespace Dualog.PortalService.Controllers.Organization.Shipping.User
{
    public class UserRepository
    {

        IDataContextFactory _dcFactory;

        public UserRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }

        public async Task<IEnumerable<UserModel>> GetUser(long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from us in dc.GetSet<DsUser>()
                         where us.Company.Id == companyId || companyId == 0
                         select new UserModel
                         {
                             Id = us.Id,
                             Name = us.Name,
                             VesselId = us.Vessel.Id,
                             VesselName = us.Vessel.VesselName,
                             CompanyId = us.Company.Id,
                             CompanyName = us.Company.Name
                         };

                return await qc.ToListAsync();
            }
        }


        public async Task<IEnumerable<UserModel>> GetShipUser(long companyId, long shipId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from us in dc.GetSet<DsUser>()
                         where us.Vessel.Id == shipId && (us.Company.Id == companyId || companyId == 0)
                         select new UserModel
                         {
                             Id = us.Id,
                             Name = us.Name,
                             VesselId = us.Vessel.Id,
                             VesselName = us.Vessel.VesselName,
                             CompanyId = us.Company.Id,
                             CompanyName = us.Company.Name
                         };

                return await qc.ToListAsync();
            }
        }

        public async Task<UserDetailModel> GetUserDetail(long companyId, long id)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from us in dc.GetSet<DsUser>()
                         where us.Id == id && (us.Company.Id == companyId || companyId == 0) 
                         select new UserDetailModel
                         {
                             Id = us.Id,
                             Name = us.Name,
                             VesselId = us.Vessel.Id,
                             VesselName = us.Vessel.VesselName,
                             CompanyId = us.Company.Id,
                             CompanyName = us.Company.Name,
                             PhoneNumber = us.PhoneNr,
                             Address = us.Address,
                             Rowstatus = us.RowStatus,

                             Membership = from ugm in us.UserGroups 
                                       select new UserGroupMemberModel
                                       {
                                           Id = ugm.Id,
                                           UserGroupName = ugm.Name
                                       }
                         };

                var user = await qc.FirstOrDefaultAsync();
                return user;
            }
        }

        
    }
}
