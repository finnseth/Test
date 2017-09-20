﻿using Dualog.Data.Entity;
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
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Controllers.Companies
{
    public class CompanyRepository
    {
        IDataContextFactory _dcFactory;

        public CompanyRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }

        public async Task<IEnumerable<CompanyInformation>> GetCompanies(Search search)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from c in dc.GetSet<DsCompany>()
                         select new CompanyInformation
                         {
                             Id = c.Id,
                             Name = c.Name
                         };

                qc = AddSearch(qc, search);

                return await qc.ToListAsync();
            }
        }

        private static IQueryable<CompanyInformation> AddSearch(IQueryable<CompanyInformation> qc, Search search)
        {
            if (search != Search.Empty)
            {
                qc = qc.Where(c => c.Name.ToUpper().Contains(search.SearchString.ToUpper()));

                if (search.Limit > 0)
                    qc = qc.Take(search.Limit);
            }

            return qc;
        }

        public static async Task InternalAddCompany(IDataContext dc, CompanyInformation companyInformation)
        {
            try
            {
                var sn = dc as ICanCreateSequenceNumbers;
                companyInformation.Id = await sn.GetSequenceNumberAsync<DsCompany>();

                dc.Add<DsCompany>(c =>
               {
                   c.Id = companyInformation.Id;
                   c.Address = companyInformation.Address;
                   c.Email = companyInformation.Email;
                   c.Manager = companyInformation.Manager;
                   c.Name = companyInformation.Name;
                   c.PhoneNumber = companyInformation.PhoneNumber;
               });


                await dc.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static async Task InternalRemoveCompany(IDataContext dc, long companyId)
        {
            await QuarantineRepository.InternalRemoveCompanyConfig(dc, companyId);
            await ServicesRepository.InternalDeleteServicesForCompany(dc, companyId);

            // Delete all vessels
            foreach (var vessel in await VesselRepository.InternalGetVessels(dc, companyId, null))
            {
                await VesselRepository.InternalDeleteVesselAsync(dc, vessel.Id);
            }

            // Delete User Groups
            await UserGroupsRepository.InternalDeleteUserGroupsForCompany(dc, companyId);

            var eq = dc as ICanExecuteQuery;

            var sql = @"DELETE FROM DS_COMPANY WHERE COM_COMPANYID = :cid";
            await eq.ExecuteSqlCommandAsync(sql, companyId);
        }
    }
}
