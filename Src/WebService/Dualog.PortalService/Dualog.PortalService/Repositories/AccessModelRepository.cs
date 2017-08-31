using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Common.Model;
using Dualog.Data.Oracle.Entity;
using Oracle.ManagedDataAccess.Client;
using Serilog;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Repositories
{
    public class AccessModelRepository
    {
        IDataContext _dc;

        public AccessModelRepository( IDataContext dc )
        {
            _dc = dc;
        }

        public IQueryable<ApAccess> GetTargetAccess( ObjectType objectType, string targetValue, params AccessRight[] accessRights )
        {
            var ot = objectType.ToString();
            var rights = accessRights.Select( i => i.ToString() ).ToArray();

            var q = from access in _dc.GetSet<ApAccess>()
                    let t = access.Target
                    where rights.Contains( access.ObjectRigth.AccessRight.Name )
                    where access.ObjectRigth.ObjectType.Name == ot
                    where t.Value == targetValue
                    select access;

            return q;
        }


        /// <summary>
        /// Grants access for a target.
        /// </summary>
        /// <param name="objectRight">The <see cref="ApObjectRight"/> to grant access to. This is returned by <see cref="GetObjectRight(IDataContext, ObjectType, AccessRight)"/>.</param>
        /// <param name="target">The <see cref="ApTarget"/> to grant access to.</param>
        /// <param name="typeInstanceId">The instance value to grant access to. The meaning of this is dependant on the object type.</param>
        /// <returns></returns>
        public async Task<ApAccess> GrantAccess( ApObjectRight objectRight, ApTarget target, string typeInstanceId )
        {
            var access = _dc.Add( new ApAccess {
                Id = await _dc.GetSequenceNumberAsync<ApAccess>(),
                ObjectRigth = objectRight, 
                Target = target,
                ObjectValue = typeInstanceId
            } );

            await _dc.SaveChangesAsync();
            Log.Debug( "Granted access to target with id {TargetId} ApAccess {AccessId}.", target.Id, access.Id );

            return access;
        }


        /// <summary>
        /// Revokes access for an instance.
        /// </summary>
        /// <param name="objectRightId">The <see cref="ApObjectRight.Id"/> to revoke access from.</param>
        /// <param name="targetId">The <see cref="ApTarget.Id"/> to revole access.</param>
        /// <param name="typeInstanceId">The instance value to revoke access to.</param>
        /// <returns><c>true</c> on success; otherwize <c>false</c>.</returns>
        public async Task<bool> RevokeAccess( long objectRightId, long targetId, string typeInstanceId )
        {
            var db = ((OracleDataContext) _dc).Database;

            string sql = 
                "delete from AP_ACCESS where " + 
                "OBR_OBJECTRIGHTID = :orid AND TAR_TARGETID = :tarid AND ACS_OBJECTVALUE = :tiid";


            var deleted = await db.ExecuteSqlCommandAsync( sql,
                                    objectRightId,
                                    targetId,
                                    typeInstanceId );

            Log.Debug( "Revoked access to target with id {TargetId} and typeInstanceId {TypeInstanceId} by deleting ApObjectRight with id {ApObjectRightId}.", targetId, typeInstanceId, objectRightId );

            return deleted > 0;         
        }


        public async Task<ApObjectRight> GetObjectRight( ObjectType objectType, AccessRight accessRight )
        {
            var ot = objectType.ToString();
            var ar = accessRight.ToString();

            var q = from objRight in _dc.GetSet<ApObjectRight>()
                    where objRight.ObjectType.Name == ot && objRight.AccessRight.Name == ar
                                                select objRight;

            return await q.FirstOrDefaultAsync();
        }
    }
}
