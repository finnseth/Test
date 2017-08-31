using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Permissions.Model;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Controllers.Permissions
{
    public class PermissionRepository
    {
        IDataContextFactory _dcFactory;
        public PermissionRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }

        public async Task<IEnumerable<PermissionDetails>> GetAllPermission(long userId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var q = CreateRegularPermissionQuery(userId, dc);
                return await q.ToListAsync();
            }
        }

        public async Task<IEnumerable<PermissionDetails>> GetAdminPermissions(long userId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var q = CreateAdminPermissionQuery(userId, dc);
                return await q.ToListAsync();
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

        private static IQueryable<PermissionDetails> CreateAdminPermissionQuery(long userId, IDataContext dc)
        {
            return from e in dc.GetSet<DsAdminUserPermission>()
                   where e.User.Id == userId && e.Function.Id > 1000 && e.AllowType > 0
                   select new PermissionDetails
                   {
                       Name = e.Function.Name,
                       AllowType = (AccessRights) e.AllowType
                   };
        }

        internal static IQueryable<PermissionDetails> CreateRegularPermissionQuery(long userId, IDataContext dc)
        {
            var groups = from ug in dc.GetSet<DsUserGroup>()
                         where ug.Users.Any(u => u.Id == userId)
                         select ug.Id;

            var q = from e in dc.GetSet<DsPermissionFunction>()
                    where e.User.Id == userId || (groups.Contains(e.UserGroup.Id))
                    group e by e.Id into g
                    from e in g
                    where e.Function.Id > 1000 && e.AllowType > 0
                    select new PermissionDetails
                    {
                        Name = e.Function.Name,
                        AllowType = (AccessRights) g.Select(j => j.AllowType).Max(),
                        Origin = (PermissionOrigin) (e.UserGroup != null ? 1 : 2)
                    };
            return q;
        }
    }

}
