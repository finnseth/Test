using Dualog.Data.Entity;
using Dualog.Data.Oracle.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Companies.Model;
using Dualog.PortalService.Controllers.Email.Setup.Quarantine;
using Dualog.PortalService.Controllers.Services;
using Dualog.PortalService.Controllers.UserGroups;
using Dualog.PortalService.Controllers.Vessels;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.PortalService.Controllers.Companies
{
    public class CompanyRepository
    {
        IDataContextFactory _dcFactory;

        public CompanyRepository( IDataContextFactory dcFactory )
        {
            _dcFactory = dcFactory;
        }

        public async Task<IEnumerable<CompanyInformation>> GetCompanies()
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from c in dc.GetSet<DsCompany>()
                         select new CompanyInformation
                         {
                             Id = c.Id,
                             Name = c.Name                             
                         };

                return await qc.ToListAsync();
            }
        }

        public static async Task InternalAddCompany( IDataContext dc, CompanyInformation companyInformation )
        {
            try
            {
                var sn = dc as ICanCreateSequenceNumbers;
                companyInformation.Id = await sn.GetSequenceNumberAsync<DsCompany>();

                dc.Add<DsCompany>( c =>
                {
                    c.Id = companyInformation.Id;
                    c.Address = companyInformation.Address;
                    c.Email = companyInformation.Email;
                    c.Manager = companyInformation.Manager;
                    c.Name = companyInformation.Name;
                    c.PhoneNumber = companyInformation.PhoneNumber;
                } );


                await dc.SaveChangesAsync();
            }
            catch( Exception exception )
            {
                throw exception;
            }
        }

        public static async Task InternalRemoveCompany( IDataContext dc, long companyId)
        {
            await QuarantineRepository.InternalRemoveCompanyConfig( dc, companyId );
            await ServicesRepository.InternalDeleteServicesForCompany( dc, companyId );

            // Delete all vessels
            foreach( var vessel in await VesselRepository.InternalGetVessels( dc, companyId ) )
            {
                await VesselRepository.InternalDeleteVesselAsync( dc, vessel.Id );
            }

            // Delete User Groups
            await UserGroupsRepository.InternalDeleteUserGroupsForCompany( dc, companyId );

            var eq = dc as ICanExecuteQuery;

            var sql = @"DELETE FROM DS_COMPANY WHERE COM_COMPANYID = :cid";
            await eq.ExecuteSqlCommandAsync( sql, companyId );        
        }
    }
}
