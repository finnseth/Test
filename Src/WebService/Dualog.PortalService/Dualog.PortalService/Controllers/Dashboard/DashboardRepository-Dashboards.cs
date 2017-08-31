using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Core.Translation;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Common.Model;
using Dualog.Data.Oracle.Entity;
using Dualog.PortalService.Core;
using Dualog.PortalService.Core.Data;
using Dualog.PortalService.Repositories;
using Newtonsoft.Json.Serialization;
using Serilog;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public partial class DashboardRepository
    {


        public async Task<IEnumerable<Dashboard>> GetDashboards( long userId )
        {
            var dashboards = await GetDashboardsForUser( userId );

            using( var dc = _dcFactory.CreateContext() )
            {
                return await GetDashboardQuery( dc ).Where( db => dashboards.Contains( db.Id ) ).ToArrayAsync();
            }
        }


        public async Task<Dashboard> GetDashboardsById( long id )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                return await GetDashboardQuery( dc ).Where( db => db.Id == id ).FirstOrDefaultAsync();
            }
        }


        private IQueryable<Dashboard> GetDashboardQuery( IDataContext dc )
        {
            var q = from db in dc.GetSet<ApDashboardInstance>()
                    select new Dashboard
                    {

                        Id = db.Id,
                        Name = db.Name
                    };
            return q;
        }


        public async Task<Dashboard> CreateDashboard( Dashboard dashboard, long userId )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var tmRepository = new TargetModelRepository( dc );
                var amRepository = new AccessModelRepository( dc );


                using( var transaction = dc.BeginTransaction() )
                {
                    try
                    {
                        Log.Information( "Adding a new Dashboard instance for user {UserId}", userId );

                        // Creating the ApDashboard instance
                        var diNew = dc.Add(new ApDashboardInstance
                        {

                            Id = await dc.GetSequenceNumberAsync<ApDashboardInstance>(),
                            Name = dashboard.Name,
                        } );

                        await dc.SaveChangesAsync();
                        Log.Debug( "Added ApDashboardInstance with {Id}", diNew.Id );


                        // Get existing or create a new target
                        var target = await tmRepository.GetTarget( TargetType.User, userId.ToString() );
                        if( target == null )
                            target = await tmRepository.CreateTarget( TargetType.User, userId.ToString() );


                        // grant access
                        var objectRight = await amRepository.GetObjectRight( ObjectType.DashboardInstance, AccessRight.Admin );
                        await amRepository.GrantAccess( objectRight, target, diNew.Id.ToString() );


                        dashboard.Id = diNew.Id;

                        transaction.Commit();
                    }
                    catch( Exception exception )
                    {
                        Log.Error( "Adding Dashboard failed with message {Message}.", exception.Message );
                        transaction.Rollback();
                        throw exception;
                    }
                }

                return dashboard;
            }
        }


        public async Task<DashboardConfig> GetDashboardConfig( LanguageManager lm )
        {
            return new DashboardConfig
            {

                Widgets = await GetWidgetNames( lm ),
                WidgetTypes = await GetWidgetTypes( lm )
            };
        }


        public async Task DeleteDashboard( long dashboardId, long userId )
        {
            using( var dc = _dcFactory.CreateContext() )
            using( var transaction = dc.BeginTransaction() )
            {
                try
                {
                    await InternalDeleteDashboard( dc, dashboardId, userId );
                    transaction.Commit();
                }
                catch( Exception ex )
                {
                    transaction.Rollback();
                }
            }
        }


        public static async Task InternalDeleteDashboard( IDataContext dataContext, long dashboardId, long userId )
        {
            try
            {
                var db = ((OracleDataContext) dataContext).Database;

                var tmRepository = new TargetModelRepository( dataContext );
                var amRepository = new AccessModelRepository( dataContext );

                string dbid = dashboardId.ToString();
                string uid = userId.ToString();


                // Get access data
                var aq = from a in dataContext.GetSet<ApAccess>()
                         where a.ObjectValue == dbid
                         where a.Target.Value == uid
                         where a.ObjectRigth.ObjectType.Name == "DashboardInstance"

                         select new
                         {
                             AccessId = a.Id,
                             ObjectRigthId = a.ObjectRigth.Id,
                             TargetId = a.Target.Id
                         };

                var accessData = await aq.FirstOrDefaultAsync();

                // Check if we got an object to delete, and skip out if we dont
                if( accessData == null )
                    return;


                // Revoke access from the dashboard
                await amRepository.RevokeAccess( accessData.ObjectRigthId, accessData.TargetId, dbid );

                // Get dashboard data
                var dbq = from di in dataContext.GetSet<ApDashboardInstance>()
                          from w in di.WidgetInstances
                          where di.Id == dashboardId
                          select w.Id;

                foreach( var widget in await dbq.ToArrayAsync() )
                {
                    await InternalDeleteWidget( dataContext, widget );
                }

                // Delete the dashboard
                string sql = "delete from AP_DASHBOARDINSTANCE " +
                          "where DBI_DASHBOARDINSTANCEID = :pk";

                await db.ExecuteSqlCommandAsync( sql, dashboardId );
            }
            catch( Exception exception )
            {
                Log.Error( "Failed to delete the dashboard with id {id}. reason: {exception}.", dashboardId, exception );
                throw exception;
            }


        }

        public async Task<Dashboard> UpdateDashboardAsync( long id, JObject json )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                (dc as IHasChangeDetection)?.EnableChangeDetection();

                // Get the dashboard from db, or return if exists.
                var entity = await dc.GetSet<ApDashboardInstance>().FirstOrDefaultAsync( di => di.Id == id );
                if( entity == null )
                    return null;


                // Create the object graph and do the patching
                var jog = new JsonObjectGraph(json, dc);
                await jog.ApplyToAsync( entity, new DefaultContractResolver() );

                var dashboard = Dashboard.FromApDashboardInstance(entity);
                dashboard.Validate( out string message );
                if( string.IsNullOrEmpty( message ) == false )
                    throw new ValidationException( message );

                await dc.SaveChangesAsync();

                return dashboard;
            }
        }


        private async Task<IEnumerable<long>> GetDashboardsForUser( long userId )
        {
            var uid = userId.ToString();

            using( var dc = _dcFactory.CreateContext() )
            {
                var amRepository = new AccessModelRepository( dc );

                var access = amRepository.GetTargetAccess( ObjectType.DashboardInstance, userId.ToString(), AccessRight.Read, AccessRight.Admin );
                var q = from a in access
                        where ((a.Target.TargetType.Name == "User" && a.Target.Value == uid)
                              || a.Target.TargetType.Name == "Global")
                        select a.ObjectValue;


                var result = await q.ToArrayAsync();
                return result.Select( a => Convert.ToInt64( a ) );
            }
        }
    }
}
