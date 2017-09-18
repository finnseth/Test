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

namespace Dualog.PortalService.Controllers.Organization.Shipping.Ship
{
    public class ShipRepository
    {

        IDataContextFactory _dcFactory;

        public ShipRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }

        public async Task<IEnumerable<ShipModel>> GetShip( long companyId )
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

                return await qc.ToListAsync();                
            }
        }

        public async Task<IEnumerable<ShipWithQuarantineModel>> GetShipWithQuarantineInfo(long companyId)
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

                return await qc.ToListAsync();
            }
        }


        public async Task<IEnumerable<AdminShipModel>> GetShipAdmin(long companyId, long shipId)
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

                return await qc.ToListAsync();
            }
        }

        
    }
}
