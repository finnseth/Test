using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Vessels.Model;
using Dualog.PortalService.Core;
using Serilog;
using System.Linq.Expressions;

namespace Dualog.PortalService.Controllers.Vessels
{
    public class VesselRepository
    {
        IDataContextFactory _dcFactory;

        public VesselRepository(IDataContextFactory dcFactory)
        {
            this._dcFactory = dcFactory;
        }


        public Task<IEnumerable<VesselDetails>> GetVessels(
                long companyId,
                Pagination pagination = null,
                Search search = null) =>

            _dcFactory.CreateContext().Use(dc =>
                InternalGetVessels(dc, companyId, pagination, search));


        public static async Task<IEnumerable<VesselDetails>> InternalGetVessels(
            IDataContext dc,
            long companyId,
            Pagination pagination = null,
            Search search = null)
        {
            var q = from v in dc.GetSet<DsVessel>()
                    where v.Company.Id == companyId
                    select new VesselDetails
                    {
                        Id = v.Id,
                        Name = v.VesselName,
                        AccountEnabled = v.AccountEnabled ?? 0,
                        IMO = v.ImoNumber,
                        Category = v.Category
                    };

            q = q.Search( search, p => p.Name ).Paginate(pagination);
            return await q.ToListAsync();
        }

        public async Task AddVesselAsync(VesselDetails vessel, long companyId)
        {
            try
            {
                using (var dc = _dcFactory.CreateContext())
                {
                    vessel.Id = await dc.GetSequenceNumberAsync<DsVessel>();

                    dc.Add<DsVessel>(v =>
                    {
                        v.Id = vessel.Id;
                        v.Company = dc.Attach<DsCompany>(cmp => cmp.Id = companyId);
                        v.VesselName = vessel.Name;
                        v.AccountEnabled = 0;
                        v.ImoNumber = vessel.IMO;
                    });

                    await dc.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                Log.Error("Failed to add a new vessel to the database for company {CompanyId}. Exception: {Exception}", companyId, exception);
                throw exception;
            }
        }

        public async Task DeleteVesselAsync(long vesselId)
        {
            using (var dc = _dcFactory.CreateContext())
            using (var transaction = dc.BeginTransaction())
            {
                try
                {
                    await InternalDeleteVesselAsync(dc, vesselId);
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();

                    // Log.Error("Failed to add a new vessel to the database for company {CompanyId}. Exception: {Exception}", companyId, exception);
                    throw exception;
                }
            }
        }

        public static async Task InternalDeleteVesselAsync(IDataContext dc, long vesselId)
        {
            var eq = dc as ICanExecuteQuery;

            string sql;


            // Delete the quarantine data for the vessel
            sql = @"DELETE FROM DS_QUARANTINE
                            WHERE VES_VESSELID = :vid";
            await eq.ExecuteSqlCommandAsync(sql, vesselId);

            // Delete the DHCP range
            sql = @"DELETE FROM DS_DHCPRANGE
                            WHERE VES_VESSELID = :vid";
            await eq.ExecuteSqlCommandAsync(sql, vesselId);

            // Delete the duacore pro config
            sql = @"DELETE FROM DS_DUACORECONFIG
                            WHERE VES_VESSELID = :vid";
            await eq.ExecuteSqlCommandAsync(sql, vesselId);

            // Delete the start pack backup
            sql = @"DELETE FROM DS_STARTPACKBACKUP
                            WHERE VES_VESSELID = :vid";
            await eq.ExecuteSqlCommandAsync(sql, vesselId);

            // Delete the web for sea config
            sql = @"DELETE FROM DS_WEB4SEACONFIG
                            WHERE VES_VESSELID = :vid";
            await eq.ExecuteSqlCommandAsync(sql, vesselId);

            // Delete the vessel
            sql = @"DELETE FROM DS_VESSEL
                            WHERE VES_VESSELID = :vid";
            await eq.ExecuteSqlCommandAsync(sql, vesselId);
        }
    }
}
