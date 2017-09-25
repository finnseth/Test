using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model;
using System.Data.Entity;
using Dualog.PortalService.Controllers.Organization.Shipping.Permission.Model;
using Dualog.PortalService.Core;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Organization.Shipping.UserGroup
{
    public class UserGroupRepository
    {

        IDataContextFactory _dcFactory;

        public UserGroupRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }

        public async Task<GenericDataModel<IEnumerable<UserGroupModel>>> GetUserGroup(long companyId)
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

                return new GenericDataModel<IEnumerable<UserGroupModel>>()
                {
                    Value = await qc.ToListAsync(),
                };
            }
        }

        public async Task<GenericDataModel<IEnumerable<UserGroupModel>>> GetShipUserGroup(long companyId, long shipId)
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

                return new GenericDataModel<IEnumerable<UserGroupModel>>()
                {
                    Value = await qc.ToListAsync(),
                };
            }
        }

        public async Task<GenericDataModel<IEnumerable<UserGroupModel>>> GetUserGroupForShip(long companyId, long shipId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                
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

                return new GenericDataModel<IEnumerable<UserGroupModel>>()
                {
                    Value = await qc.ToListAsync(),
                };
            }
        }

        public async Task<GenericDataModel<UserGroupDetailModel>> GetUserGroupDetail(long companyId, long usgId)
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
                                       },
                             Permissions = from pf in ug.Permissions
                                           select new PermissionDetailModel
                                           {
                                               Name = pf.Function.Name,
                                               AllowType = (AccessRights) pf.AllowType
                                           }
                         };

                return new GenericDataModel<UserGroupDetailModel>()
                {
                    Value = await qc.FirstOrDefaultAsync(),
                };

            }
        }


        public async Task AddUserGroup(UserGroupDetailModel userGroup, long companyId, long? vesselId = null)
        {
            using (var dc = _dcFactory.CreateContext())
            using (var transaction = dc.BeginTransaction())
            {
                (dc as IHasChangeDetection)?.EnableChangeDetection();
                var seq = dc as ICanCreateSequenceNumbers;

                var nUserGroup = dc.Add<DsUserGroup>(async ug =>
                {

                    ug.Id = await seq.GetSequenceNumberAsync<DsUserGroup>();
                    ug.Company = dc.Attach<DsCompany>(c => c.Id = companyId);
                    ug.Name = userGroup.Name;
                    ug.Vessel = vesselId != null ? dc.Attach<DsVessel>(v => v.Id = vesselId.Value) : null;
                    ug.Permissions = new List<DsPermissionFunction>();
                });

                userGroup.Id = nUserGroup.Id;

                await dc.SaveChangesAsync();


                if (userGroup.Permissions != null && userGroup.Permissions.Any())
                {
                    var permissionNames = userGroup.Permissions
                                                   .Select(p => p.Name.ToLower())
                                                   .ToArray();

                    var functions = await dc.GetSet<DsPermissionFunction>()
                                            .Include(pf => pf.Function)
                                            .Where(pf => pf.UserGroup.Id == userGroup.Id)
                                            .Where(f => permissionNames.Contains(f.Function.Name.ToLower()))
                                            .ToDictionaryAsync(k => k.Function.Name.ToLower());



                    foreach (var permission in userGroup.Permissions)
                    {
                        if (functions.TryGetValue(permission.Name.ToLower(), out var func) == false)
                        {
                            transaction.Rollback();
                            throw new ValidationException($"The permission with name {permission.Name} does not exists.");
                        }

                        func.AllowType = (int)permission.AllowType;
                    }
                }

                transaction.Commit();
                await dc.SaveChangesAsync();
            }
        }

        public async Task<UserGroupDetailModel> ChangeUserGroupAsync(long id, JObject patch)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                (dc as IHasChangeDetection)?.EnableChangeDetection();

                var userGroup = dc.GetSet<DsUserGroup>()
                                  .Include("Permissions")
                                  .Include("Permissions.Function")
                                  .Where(ug => ug.Id == id)
                                  .FirstOrDefault();

                if (userGroup == null)
                    throw new NotFoundException();


                var jog = new JsonObjectGraph(patch, dc);



                jog.LookupObjectById = (path, value) =>
                {
                    if (path == "/permissions/update")
                    {
                        var permission = userGroup.Permissions.FirstOrDefault(p => p.Function.Name.ToLower() == value.ToString().ToLower());
                        if (permission == null)
                            throw new ValidationException($"Permission with the name {value.ToString()} was not found.");

                        return permission;
                    }

                    return null;
                };


                await jog.ApplyToAsync(userGroup, new DefaultContractResolver());

                var ugDetails = UserGroupDetailModel.FromDsUSerGroup(userGroup);
                if (ugDetails.Validate(out var message) == false)
                    throw new ValidationException(message);


                await dc.SaveChangesAsync();


                return ugDetails;
            }
        }

        public async Task DeleteUserGroup(long companyId, long userGroupId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var eq = dc as ICanExecuteQuery;

                var sql = @"UPDATE DS_USERGROUP 
                            SET USG_ROWSTATUS = 2
                            WHERE COM_COMPANYID = :cmp AND USG_USERGROUPID = :ugid";
                await eq.ExecuteSqlCommandAsync(sql, companyId, userGroupId);
            }
        }


        public static async Task InternalDeleteUserGroupsForCompany(IDataContext dataContext, long companyId)
        {
            var eq = dataContext as ICanExecuteQuery;

            var sql = @"DELETE FROM DS_USERGROUP WHERE COM_COMPANYID = :cmp";

            await eq.ExecuteSqlCommandAsync(sql, companyId);
        }

    }
}
