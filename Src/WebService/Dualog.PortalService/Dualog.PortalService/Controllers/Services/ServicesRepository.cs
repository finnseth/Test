using System;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Services.Model;
using Newtonsoft.Json.Linq;
using System.Data.Entity;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Dualog.PortalService.Controllers.Services
{
    public class ServicesRepository
    {
        IDataContextFactory _dcFactory;

        public ServicesRepository( IDataContextFactory dcFactory )
        {
            _dcFactory = dcFactory;
        }

        public async Task<ServiceDetails> GetServiceAsync( long companyId, long id )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var q = from s in dc.GetSet<DsService>()
                        where s.Company.Id == companyId && s.Id == id
                        select new ServiceDetails{
                            Id = s.Id,
                            Name = s.Name,
                            Port = s.Port,
                            Ports = s.Ports,
                            Protocol = s.Protocol
                        };

                var service = await q.FirstOrDefaultAsync();
                if( service == null )
                    throw new NotFoundException();

                return service;
            }
        }

        public async Task<IEnumerable<ServiceDetails>> GetServicesAsync( long companyId, long? vessel )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var pq = dc.GetSet<DsService>()
                           .Where( s => s.Company.Id == companyId );

                if( vessel != null )
                    pq = pq.Where( s => s.Vessel.Id == vessel.Value );

                var q = from s in pq
                        select new ServiceDetails{
                            Id = s.Id,
                            Name = s.Name,
                            Port = s.Port,
                            Ports = s.Ports,
                            Protocol = s.Protocol
                        };


                return (await q.ToListAsync()).AsEnumerable();
            }
        }


        public async Task<ServiceDetails> AddService( ServiceDetails serviceDetails, long companyId, long? vesselId = null )
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


        public async Task<ServiceDetails> PatchService( long companyId, long id, JObject json )
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

                ServiceDetails serviceDetails = ServiceDetails.FromDsService( service );
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
