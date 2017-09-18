using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Email.Setup.Quarantine.Model;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Dualog.PortalService.Controllers.Email.Setup.Quarantine
{
    public class QuarantineRepository
    {
        private readonly IDataContextFactory _dcFactory;

        public QuarantineRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }


        public async Task<IEnumerable<QuarantineCompanyModel>> GetCompanyConfig(long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from e in dc.GetSet<DsQuarantine>()
                         where (e.Company.Id == companyId || companyId == 0) && e.Vessel == null
                         select new QuarantineCompanyModel
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

                return await qc.ToListAsync();
            }
        }


        public async Task<IEnumerable<QuarantineVesselModel>> GetVesselConfigurationList(long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = GetVesselConfigQuery(companyId, dc);
                return await qc.ToListAsync();
            }
        }

        public async Task<QuarantineVesselModel> GetVesselConfiguration(long vesselId, long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from e in GetVesselConfigQuery(companyId, dc)
                         where e.VesselId == vesselId
                         select e;

                return await qc.FirstOrDefaultAsync();
            }
        }

        private static IQueryable<QuarantineVesselModel> GetVesselConfigQuery(long companyId, IDataContext dc)
        {
            return from e in dc.GetSet<DsQuarantine>()
                   where (e.Company.Id == companyId || companyId == 0)  && e.Vessel != null
                   select new QuarantineVesselModel
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


        public async Task<QuarantineCompanyModel> PatchCompanyConfigAsync(JObject config, long companyId , long quarantineId)
        {
            using (var dc = _dcFactory.CreateContext( ))
            {
                (dc as IHasChangeDetection)?.EnableChangeDetection();

                JsonObjectGraph jog = new JsonObjectGraph(config, dc);

                var entity = await dc.GetSet<DsQuarantine>().Include( q => q.Vessel )
                                     .Where(q => (q.Company.Id == companyId || companyId == 0) && q.Vessel == null && q.Id ==quarantineId) 
                                     .FirstOrDefaultAsync();

                if (entity == null)
                    throw new NotFoundException();



                await jog.ApplyToAsync(entity, new DefaultContractResolver() );

                var result = QuarantineCompanyModel.FromDsQuarantine(entity);
                if (result.Validate(out var message) == false)
                    throw new ValidationException(message);

                await dc.SaveChangesAsync();
                return result;
            }
        }

        public async Task<QuarantineVesselModel> PatchVesselConfigAsync(JObject config, long companyId, long quarantineId)
        {

            using (var dc = _dcFactory.CreateContext())
            {
                (dc as IHasChangeDetection)?.EnableChangeDetection();

                JsonObjectGraph jog = new JsonObjectGraph(config, dc);

                var entity = await dc.GetSet<DsQuarantine>().Where(q => (q.Company.Id == companyId || companyId == 0) 
                                                                        && q.Vessel != null 
                                                                        && q.Id  == quarantineId).FirstOrDefaultAsync();

                if (entity == null)
                    throw new NotFoundException();

                await jog.ApplyToAsync(entity, new DefaultContractResolver() );

                var result = QuarantineVesselModel.FromDsQuarantine(entity);
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
