using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using System.Collections.Generic;
using Dualog.PortalService.Controllers.Network.BandwidthRestriction.Model;
using Dualog.Data.Oracle.Shore.Model;
using System.Data.Entity;
using Dualog.PortalService.Properties;
using Oracle.ManagedDataAccess.Client;
using Dualog.Data.Oracle.Entity;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Network.BandwidthRestriction
{
    public class BandwidthRestrictionRepository
    {
        IDataContextFactory _dcFactory;

        public BandwidthRestrictionRepository( IDataContextFactory dcFactory )
        {
            _dcFactory = dcFactory;
        }
        public async Task<GenericDataModel<IEnumerable<CompanyRule>>> GetRulesForCompany( long companyId )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var q = from r in dc.GetSet<DsOnlineAccessRight>()
                        where r.Company.Id == companyId && r.Vessel == null && r.ComputerRule == true 
                        select new CompanyRule
                        {
                            Id = r.Id,
                            Method = r.Carrier == null ? null : new Method
                            {
                                Id = r.Carrier.Id,
                                Name = r.Carrier.Name
                            },
                            Description = r.Description,
                            SourceComputer = r.SourceComputer,
                            Priority = r.Priority ?? 0,
                            Service = r.Service == null ? null : new Service
                            {
                                Id = r.Service.Id,
                                Name = r.Service.Name
                            },
                            Destinations = from d in r.Destinations
                                           select d.Destination
                        };

                return new GenericDataModel<IEnumerable<CompanyRule>>()
                {
                    Value = await q.ToListAsync(),
                };
            }
        }


        public async Task<CompanyRule> AddRightsForCompanyAsync( long companyId, CompanyRule companyRule )
        {
            using( var dc = _dcFactory.CreateContext() )
            using( var transaction = dc.BeginTransaction() )
            {
                try
                {
                    await InternalAddRightsForCompany( dc, companyId, companyRule );
                    transaction.Commit();
                }
                catch( Exception exception )
                {
                    transaction.Rollback();
                    throw exception;
                }
            }

            return companyRule;
        }

        public static async Task InternalAddRightsForCompany( IDataContext dc, long companyId, CompanyRule companyRule )
        {
            var seq = dc as ICanCreateSequenceNumbers;


            var accessRight = dc.Add<DsOnlineAccessRight>( async ar =>
            {
                ar.Id = await seq.GetSequenceNumberAsync<DsOnlineAccessRight>();
                ar.Carrier = companyRule.Method != null ? dc.Attach<DsCarrier>( c => c.Id = companyRule.Method.Id ) : null;
                ar.Company = dc.Attach<DsCompany>( c => c.Id = companyId );
                ar.ComputerRule = false;
                ar.Description = companyRule.Description;
                ar.Priority = companyRule.Priority;
                ar.Service = companyRule.Service != null ? dc.Attach<DsService>( s => s.Id = companyRule.Service.Id ) : null;
                ar.SourceComputer = companyRule.SourceComputer;
            });


            if( companyRule.Destinations != null && companyRule.Destinations.Any() )
            {
                var pkList = await (dc as ICanCreateSequenceNumbers).GetSequenceNumbersAsync<DsOnlineAccessDestination>( companyRule.Destinations.Count() );
                var index = 0;

                foreach( var d in companyRule.Destinations )
                {
                    accessRight.Destinations.Add( new DsOnlineAccessDestination
                    {
                        Id = pkList[ index++ ],
                        Destination = d
                    } );
                }
            }

        }


        public async Task<GenericDataModel<IEnumerable<VesselRule>>> GetRulesForVessel( long companyId, long vesselId )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                (dc as OracleDataContext).BindByNameOnExecuteQuery = true;

                var eq = dc as ICanExecuteQuery;

                var qr = await eq.ExecuteSqlCommandAsync<irules>( Resources.sqlVesselDCPRules,
                            new OracleParameter( "vesselid", vesselId ),
                            new OracleParameter( "companyid", companyId ) );

                var mids = qr.Where( m => m.cvc_carriervesselid != null )
                             .Select( m => m.cvc_carriervesselid)
                             .Distinct()
                             .ToArray();

                var arid = qr.Select( m => m.ola_onlineaccessrightid )
                             .ToArray();

                var methods = await (from m in dc.GetSet<DsCarrierVesselConfig>()
                                     where mids.Contains( m.Id )
                                     select new Method
                                     {
                                         Id = m.Id,
                                         Name = m.Description
                                     }).ToDictionaryAsync( k => k.Id );

                var services = await (from s in dc.GetSet<DsService>()
                                      select new Service
                                      {
                                          Id = s.Id,
                                          Name = s.Name
                                      }).ToDictionaryAsync( k => k.Id );

                var destinations = (await (from d in dc.GetSet<DsOnlineAccessDestination>()
                                          where arid.Contains( d.AccessRight.Id)
                                          select new
                                          {
                                              d.AccessRight.Id,
                                              d.Destination

                                          }).ToListAsync()).ToLookup( k => k.Id );

                var q = from row in qr
                        let mid = row.cvc_carriervesselid
                        let me = methods.ContainsKey( mid ?? 0 )
                        let se = services.ContainsKey( row.ser_serviceid ?? 0 )
                        select new VesselRule
                        {
                            Description = row.ola_description,
                            Method = me ? methods[ mid ?? 0 ] : null,
                            Id = row.ola_onlineaccessrightid ?? 0,
                            IsActive = (row.cvc_rowsstatus ?? 0) == 0,
                            Priority = row.ola_priority ?? 0,
                            Service = se ? services[ row.ser_serviceid ?? 0] : null,
                            SourceComputer = row.ola_sourcecomputer,
                            IsCompanyRule = row.ves_vesselid == null,
                            Destinations = destinations[ row.ola_onlineaccessrightid ?? 0].Select( i => i.Destination )
                        };

                return new GenericDataModel<IEnumerable<VesselRule>>()
                {
                    Value = q.ToList(),
                };
                       }
        }

        private class irules
        {
            public string ola_description { get; set; }
            public long? cvc_carriervesselid { get; set; }
            public string ola_sourcecomputer { get; set; }
            public long? ser_serviceid { get; set; }
            public short? ola_priority { get; set; }
            public long? cat_carrierid { get; set; }
            public short? ola_computerrule { get; set; }
            public long? ola_onlineaccessrightid { get; set; }
            public long? ola_olaid { get; set; }
            public long? usg_usergroupid { get; set; }
            public long? ves_vesselid { get; set; }
            public short? cvc_rowsstatus { get; set; }
            public string old_destination { get; set; }
        }
    }
}
