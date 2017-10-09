using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Permission.Model;
using Dualog.PortalService.Core;
using Dualog.PortalService.Models;
using Dapper;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Permission
{
    public class PermissionRepository
    {
        IDataContextFactory _dcFactory;
        public PermissionRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }

        public async Task<GenericDataModel<IEnumerable<PermissionDetailModel>>> GetAllPermission(long userId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var q = CreateRegularPermissionQuery(userId, dc);

                return new GenericDataModel<IEnumerable<PermissionDetailModel>>()
                {
                    Value = await q.ToListAsync()
                };

            }
        }

        public async Task<GenericDataModel<IEnumerable<PermissionDetailModel>>> GetAdminPermission(long userId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var q = CreateAdminPermissionQuery(userId, dc);

                return new GenericDataModel<IEnumerable<PermissionDetailModel>>()
                {
                    Value = await q.ToListAsync()
                };

            }
        }

        public async Task<Boolean> CheckPermission(long userId, string resourceName, AccessRights access, bool isAdmin)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var pq = isAdmin == false ? CreateRegularPermissionQuery(userId, dc) : CreateAdminPermissionQuery(userId, dc);
                var q = from p in pq
                        where p.Name.ToLower() == resourceName.ToLower() && access <= p.AllowType
                        select p;

                bool result = await q.AnyAsync();
                return result;
            }
        }

        private static IQueryable<PermissionDetailModel> CreateAdminPermissionQuery(long userId, IDataContext dc)
        {
            return from e in dc.GetSet<DsAdminUserPermission>()
                   where e.User.Id == userId && e.Function.Id > 1000 && e.AllowType > 0
                   select new PermissionDetailModel
                   {
                       Name = e.Function.Name,
                       AllowType = (AccessRights) e.AllowType
                   };
        }

        internal static IQueryable<PermissionDetailModel> CreateRegularPermissionQuery(long userId, IDataContext dc)
        {
            var groups = from ug in dc.GetSet<DsUserGroup>()
                         where ug.Users.Any(u => u.Id == userId)
                         select ug.Id;

            var q = from e in dc.GetSet<DsPermissionFunction>()
                    where e.User.Id == userId || (groups.Contains(e.UserGroup.Id))
                    group e by e.Id into g
                    from e in g
                    where e.Function.Id > 1000 && e.AllowType > 0
                    select new PermissionDetailModel
                    {
                        Name = e.Function.Name,
                        AllowType = (AccessRights) g.Select(j => j.AllowType).Max(),
                        Origin = e.UserGroup != null ? e.UserGroup.Name : null
                    };

            return q.OrderBy( p => p.Name );
        }

        public static async Task InternalGrantPermissionAsync(IDataContext dc, long userId, string permission, AccessRights rights)
        {
            var conn = (dc as IHasDataConnection).GetDataConnection();

            var sql = @"INSERT INTO DS_PERMISSIONFUNCTION (PEF_PERMISSIONFUNCID, FUN_FUNCTIONID, USR_USERID, PEF_ALLOWTYPE)
			            SELECT DS_PERMISSIONFUNCTION_SEQ.nextval, F.FUN_FUNCTIONID, :u, :a
	 	                FROM DS_FUNCTION F 
	 	                WHERE LOWER(F.FUN_FUNCTIONNAME) = LOWER(:n)";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("u", userId);
            parameters.Add("a", (int)rights);
            parameters.Add("n", permission);

            await conn.ExecuteAsync(sql,  parameters);
        }
    }

}
