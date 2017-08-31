using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Quarantine.Model;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Dualog.PortalService.Controllers.Quarantine
{
    public class QuarantineRepository
    {
        private readonly IDataContextFactory _dcFactory;

        public QuarantineRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }


        public async Task<QuarantineCompanyConfig> GetCompanyConfig(long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from e in dc.GetSet<DsQuarantine>()
                         where e.Company.Id == companyId && e.Vessel == null
                         select new QuarantineCompanyConfig
                         {
                             QuarantineId = e.Id,
                             MaxBodyLength = e.MaxBodyLength,
                             NotificationOnHoldAdmins = e.NotificationOnHoldAdmins,
                             NotificationOnHoldOriginal = e.NotificationOnHoldOriginal,
                             NotificationOnHoldPostmaster = e.NotificationOnHoldPostmaster,
                             NotificationOnHoldRecipient = e.NotificationOnHoldRecipient,
                             NotificationSender = e.NotificationSender,
                             OnHoldCrew = e.OnHoldCrew,
                             OnHoldDuration = e.OnHoldDuration,
                             OnHoldStationaryUser = e.OnHoldStationaryUser,
                         };

                return await qc.FirstOrDefaultAsync();
            }
        }


        public async Task<IEnumerable<QuarantineVesselConfig>> GetVesselConfigurationList(long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = GetVesselConfigQuery(companyId, dc);
                return await qc.ToListAsync();
            }
        }

        public async Task<QuarantineVesselConfig> GetVesselConfiguration(long vesselId, long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from e in GetVesselConfigQuery(companyId, dc)
                         where e.VesselId == vesselId
                         select e;

                return await qc.FirstOrDefaultAsync();
            }
        }

        private static IQueryable<QuarantineVesselConfig> GetVesselConfigQuery(long companyId, IDataContext dc)
        {
            return from e in dc.GetSet<DsQuarantine>()
                   where e.Company.Id == companyId  && e.Vessel != null
                   select new QuarantineVesselConfig
                   {
                       QuarantineId = e.Id,
                       VesselId = e.Vessel.Id,
                       VesselName = e.Vessel.VesselName,
                       MaxBodyLength = e.MaxBodyLength,
                       NotificationOnHoldAdmins = e.NotificationOnHoldAdmins,
                       NotificationOnHoldOriginal = e.NotificationOnHoldOriginal,
                       NotificationOnHoldPostmaster = e.NotificationOnHoldPostmaster,
                       NotificationOnHoldRecipient = e.NotificationOnHoldRecipient,
                       NotificationSender = e.NotificationSender,
                       OnHoldCrew = e.OnHoldCrew,
                       OnHoldDuration = e.OnHoldDuration,
                       OnHoldStationaryUser = e.OnHoldStationaryUser,
                       UseThisLevel = e.UseThisLevel
                   };
        }


        public async Task<QuarantineCompanyConfig> PatchCompanyConfigAsync(JObject config, long id )
        {


            using (var dc = _dcFactory.CreateContext( ))
            {
                JsonObjectGraph jog = new JsonObjectGraph(config, dc);
                //jog.AddPropertyMap( "/" )


                var entity = await dc.GetSet<DsQuarantine>().Include( q => q.Vessel )
                                     .Where(q => q.Company.Id == id && q.Vessel == null)
                                     .FirstOrDefaultAsync();

                await jog.ApplyToAsync(entity, new DefaultContractResolver() );

                var result = QuarantineCompanyConfig.FromDsQuarantine(entity);
                if (result.Validate(out var message) == false)
                    throw new ValidationException(message);

                await dc.SaveChangesAsync();
                return result;
            }
        }

        public async Task<QuarantineVesselConfig> PatchVesselConfigAsync(JObject config, long companyId, long vesselId)
        {

            using (var dc = _dcFactory.CreateContext())
            {
                JsonObjectGraph jog = new JsonObjectGraph(config, dc);

                var entity = await dc.GetSet<DsQuarantine>().Where(q => q.Company.Id == companyId && q.Vessel.Id  == vesselId).FirstOrDefaultAsync();
                await jog.ApplyToAsync(entity, new DefaultContractResolver() );

                var result = QuarantineVesselConfig.FromDsQuarantine(entity);
                if (result.Validate(out var message) == false)
                    throw new ValidationException(message);

                await dc.SaveChangesAsync();

                return result;
            }
        }

        public static async Task InternalRemoveCompanyConfig( IDataContext dc, long companyId )
        {
            var eq = dc as ICanExecuteQuery;

            var sql = @"DELETE FROM DS_QUARANTINE WHERE COM_COMPANYID = :cid";
            await eq.ExecuteSqlCommandAsync( sql, companyId );
        }
    }
}
