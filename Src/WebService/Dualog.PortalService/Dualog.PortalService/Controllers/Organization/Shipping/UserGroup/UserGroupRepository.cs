using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Data.Entity;

namespace Dualog.PortalService.Controllers.Organization.Shipping.UserGroup
{
    public class UserGroupRepository
    {

        IDataContextFactory _dcFactory;

        public UserGroupRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }

        public async Task<IEnumerable<UserGroupModel>> GetUserGroup(long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from ug in dc.GetSet<DsUserGroup>()
                         where (ug.RowStatus == 0 || ug.RowStatus == 1) 
                         && (ug.Company.Id == companyId || companyId == 0)
                         select new UserGroupModel
                         {
                             Id = ug.Id,
                             Name = ug.Name,
                             Rowstatus = ug.RowStatus,
                             VesselId = ug.Vessel.Id,
                             VesselName = ug.Vessel.VesselName,
                             CompanyId = ug.Company.Id,
                             CompanyName = ug.Company.Name
                         };

                return await qc.ToListAsync();
            }
        }

        public async Task<IEnumerable<UserGroupModel>> GetShipUserGroup(long companyId, long shipId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from ug in dc.GetSet<DsUserGroup>()
                         where  (ug.RowStatus == 0 || ug.RowStatus == 1)
                         && ug.Vessel.Id == shipId 
                         && (ug.Company.Id == companyId || companyId == 0) 
                         select new UserGroupModel
                         {
                             Id = ug.Id,
                             Name = ug.Name,
                             Rowstatus = ug.RowStatus,
                             VesselId = ug.Vessel.Id,
                             VesselName = ug.Vessel.VesselName,
                             CompanyId = ug.Company.Id,
                             CompanyName = ug.Company.Name
                         };

                return await qc.ToListAsync();
            }
        }

        public async Task<IEnumerable<UserGroupModel>> GetUserGroupForShip(long companyId, long shipId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                //var t = dc as ICanExecuteQuery;
                //var something = t.ExecuteSqlCommandAsync<String[]>("");
                
                var qc = from ves in dc.GetSet<DsVessel>()
                         join ug in dc.GetSet<DsUserGroup>() on ves.Company.Id equals ug.Company.Id
                         where ves.Id == shipId 
                         && (ug.RowStatus == 0 || ug.RowStatus == 1) 
                         && (ug.Vessel.Id == shipId || ug.Vessel == null) 
                         && (ves.Company.Id == companyId || companyId == 0)
                         select new UserGroupModel
                         {
                             Id = ug.Id,
                             Name = ug.Name,
                             Rowstatus = ug.RowStatus,
                             VesselId = ug.Vessel.Id,
                             VesselName = ug.Vessel.VesselName,
                             CompanyId = ug.Company.Id,
                             CompanyName = ug.Company.Name
                         };

                return await qc.ToListAsync();
            }
        }

        public async Task<IEnumerable<UserGroupDetailModel>> GetUserGroupDetail(long companyId, long usgId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from ug in dc.GetSet<DsUserGroup>()
                         where ug.Company.Id == companyId
                         && ug.Id == usgId
                         select new UserGroupDetailModel
                         {
                             Id = ug.Id,
                             Name = ug.Name,
                             Description = ug.Description,
                             Rowstatus = ug.RowStatus,
                             VesselId = ug.Vessel.Id,
                             CompanyId = ug.Company.Id,
                             VesselName = ug.Vessel.VesselName,
                             CompanyName = ug.Company.Name,

                             Members = from ugm in ug.Users
                                       select new UserMemberModel
                                       {
                                           Id = ugm.Id,
                                           UserName = ugm.Name
                                       }
                         };

                return await qc.ToListAsync();
            }
        }

    }
}
