using System;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Network.Setup.Services.Model;
using Newtonsoft.Json.Linq;
using System.Data.Entity;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Network.Setup.Services
{
    public class ServiceRepository
    {
        IDataContextFactory _dcFactory;

        public ServiceRepository( IDataContextFactory dcFactory )
        {
            _dcFactory = dcFactory;
        }

        public async Task<GenericDataModel<ServiceDetailModel>> GetServiceAsync( long companyId, long id )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var q = from s in dc.GetSet<DsService>()
                        where (s.Company.Id == companyId || companyId == 0) && s.Id == id
                        select new ServiceDetailModel{
                            Id = s.Id,
                            Name = s.Name,
                            Port = s.Port,
                            Ports = s.Ports,
                            Protocol = s.Protocol,
                            Company = s.Company.Name,
                            Ship = s.Vessel.VesselName,
                        };

                var service = await q.FirstOrDefaultAsync();
                if( service == null )
                    throw new NotFoundException();

                return new GenericDataModel<ServiceDetailModel>()
                {
                    Value = service,
                };

            }
        }

        public async Task<GenericDataModel<IEnumerable<ServiceDetailModel>>> GetServicesAsync( long companyId, long? vessel )
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var pq = dc.GetSet<DsService>()
                           .Where(s => s.Company.Id == companyId || companyId == 0);

                if (vessel != null)
                    pq = pq.Where(s => s.Vessel.Id == vessel.Value);

                var q = from s in pq
                        select new ServiceDetailModel {
                            Id = s.Id,
                            Name = s.Name,
                            Port = s.Port,
                            Ports = s.Ports,
                            Protocol = s.Protocol
                        };

                return new GenericDataModel<IEnumerable<ServiceDetailModel>>()
                {
                    Value = await q.ToListAsync()
                };

            }
        }


        public async Task<ServiceDetailModel> AddService( ServiceDetailModel serviceDetails, long companyId, long? vesselId = null )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var seq = dc as ICanCreateSequenceNumbers;
                serviceDetails.Id = await seq.GetSequenceNumberAsync<DsService>();


                dc.Add<DsService>( s => {
                    s.Company = dc.Attach<DsCompany>( c => c.Id = companyId );

                    if( vesselId != null )
                        s.Vessel = dc.Attach<DsVessel>( v => v.Id = vesselId.Value );

                    s.Id = serviceDetails.Id;
                    s.Name = serviceDetails.Name;
                    s.Port = serviceDetails.Port;
                    s.Ports = serviceDetails.Ports;
                    s.Protocol = serviceDetails.Protocol;
                } );

                await dc.SaveChangesAsync();
            }

            return serviceDetails;
        }


        public async Task<ServiceDetailModel> PatchService( long companyId, long id, JObject json )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                (dc as IHasChangeDetection)?.EnableChangeDetection();

                var service =  await dc.GetSet<DsService>()
                                       .Where( s => s.Id == id && s.Company.Id == companyId )
                                       .FirstOrDefaultAsync();

                if( service == null )
                    throw new NotFoundException();

                var jog = new JsonObjectGraph(json, dc);
                await jog.ApplyToAsync( service, new DefaultContractResolver() );

                ServiceDetailModel serviceDetails = ServiceDetailModel.FromDsService( service );
                if( serviceDetails.Validate( out var message ) == false )
                    throw new ValidationException( message );

                await dc.SaveChangesAsync();

                return serviceDetails;
            }
        }


        public async Task<bool> DeleteServiceAsync( long companyId, long id )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                if( dc is ICanExecuteQuery eq )
                {
                    var sql = @"DELETE FROM DS_SERVICE
                            WHERE COM_COMPANYID = :cid AND SER_SERVICEID = :sid";

                    await eq.ExecuteSqlCommandAsync( sql, companyId, id );
                }
            }

            return true;
        }

        public static async Task InternalDeleteServicesForCompany( IDataContext dataContext, long companyId )
        {
            if( dataContext is ICanExecuteQuery eq )
            {
                var sql = @"DELETE FROM DS_SERVICE
                            WHERE COM_COMPANYID = :cid";

                await eq.ExecuteSqlCommandAsync( sql, companyId );
            }
        }
    }
}
