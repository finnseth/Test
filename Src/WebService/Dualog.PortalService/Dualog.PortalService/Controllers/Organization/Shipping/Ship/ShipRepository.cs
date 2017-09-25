using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Ship.Model;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Dualog.PortalService.Core;
using Dualog.PortalService.Models;
using Serilog;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Ship
{
    public class ShipRepository
    {

        IDataContextFactory _dcFactory;

        public ShipRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }



        public async Task<GenericDataModel<IEnumerable<ShipModel>>> GetShip(long companyId,
                Pagination pagination = null,
                Search search = null)
        { 

            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from ves in dc.GetSet<DsVessel>()
                         where ves.Company.Id == companyId || companyId == 0 
                         select new ShipModel
                         {
                             Id = ves.Id,
                             Name = ves.VesselName,
                             Company = ves.Company.Name,
                             Phone = ves.PhoneNumber,
                             IsOfficeInstallation = ves.OfficeInstallation,
                             InstalledVersion = ves.InstalledVersion,
                             DatabaseVersion = ves.DbVersion,
                             HardwareID = ves.VesselPcId,
                             RadioCallSignal = ves.RadioCallSignal,
                             ImoNumber = ves.ImoNumber,
                             DialinPassword = ves.DialInPassword,
                             AccountEnabled = ves.AccountEnabled ?? 0
                         };

                return new GenericDataModel<IEnumerable<ShipModel>>()
                {
                    Value = await qc.ToListAsync(),
                };

            }
        }

        public async Task<GenericDataModel<IEnumerable<ShipWithQuarantineModel>>> GetShipWithQuarantineInfo(long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from qu in dc.GetSet<DsQuarantine>()
                         where (qu.Vessel.Company.Id == companyId || companyId == 0) && qu.Vessel != null
                         select new ShipWithQuarantineModel
                         {
                             Id = qu.Vessel.Id,
                             Name = qu.Vessel.VesselName,
                             Company = qu.Vessel.Company.Name,
                             Phone = qu.Vessel.PhoneNumber,
                             IsOfficeInstallation = qu.Vessel.OfficeInstallation,
                             InstalledVersion = qu.Vessel.InstalledVersion,
                             DatabaseVersion = qu.Vessel.DbVersion,
                             HardwareID = qu.Vessel.VesselPcId,
                             RadioCallSignal = qu.Vessel.RadioCallSignal,
                             ImoNumber = qu.Vessel.ImoNumber,
                             DialinPassword = qu.Vessel.DialInPassword,
                             QuarantineLocalChanges = qu.UseThisLevel,
                             AccountEnabled = qu.Vessel.AccountEnabled ?? 0
                         };

                return new GenericDataModel<IEnumerable<ShipWithQuarantineModel>>()
                {
                    Value = await qc.ToListAsync(),
                };
            }
        }


        public async Task<GenericDataModel<AdminShipModel>> GetShipAdmin(long companyId, long shipId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from ves in dc.GetSet<DsVessel>()
                         where ves.Id == shipId && (ves.Company.Id == companyId || companyId == 0)
                         select new AdminShipModel
                         {
                             Id = ves.Id,
                             Name = ves.VesselName,
                             Company = ves.Company.Name,
                             Phone = ves.PhoneNumber,
                             IsOfficeInstallation = ves.OfficeInstallation,
                             InstalledVersion = ves.InstalledVersion,
                             DatabaseVersion = ves.DbVersion,
                             HardwareID = ves.VesselPcId,
                             RadioCallSignal = ves.RadioCallSignal,
                             ImoNumber = ves.ImoNumber,
                             DialinPassword = ves.DialInPassword,
                             AccountEnabled = (short)ves.AccountEnabled,
                             Billing = (long)ves.CustomerNumber,
                             Category = (ShipCategory)ves.Category,
                             DualogUnit = ves.VesselType.Id,
                             TimeAdded = ves.TimeAdded
                         };

                return new GenericDataModel<AdminShipModel>()
                {
                    Value = await qc.FirstOrDefaultAsync(),
                };
            }
        }

        public static async Task<IEnumerable<ShipModel>> InternalGetVessels(
            IDataContext dc,
            long companyId,
            Pagination pagination = null,
            Search search = null)
        {
            var q = from v in dc.GetSet<DsVessel>()
                    where v.Company.Id == companyId
                    select new ShipModel
                    {
                        Id = v.Id,
                        Name = v.VesselName,
                        AccountEnabled = v.AccountEnabled ?? 0,
                        RadioCallSignal = v.RadioCallSignal
                    };


            q = q.Search(search, p => p.Name).Paginate(pagination);

            return await q.ToListAsync();
        }

        public async Task AddVesselAsync(ShipModel vessel, long companyId)
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
                        v.RadioCallSignal = vessel.RadioCallSignal;
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
