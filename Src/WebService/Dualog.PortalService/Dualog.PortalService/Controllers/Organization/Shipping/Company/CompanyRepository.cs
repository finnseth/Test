using Dualog.Data.Entity;
using Dualog.Data.Oracle.Entity;
using Dualog.Data.Oracle.Shore.Model;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dualog.PortalService.Core;
using Dualog.PortalService.Controllers.Organization.Shipping.Company.Model;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using Dualog.PortalService.Models;
using Dualog.PortalService.Controllers.Email.Setup.Quarantine;
using Dualog.PortalService.Controllers.Network.Setup.Services;
using Dualog.PortalService.Controllers.Organization.Shipping.Ship;
using Dualog.PortalService.Controllers.Organization.Shipping.UserGroup;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Company
{
    public class CompanyRepository
    {

        IDataContextFactory _dcFactory;

        public CompanyRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }


        public async Task<GenericDataModel<IEnumerable<CompanyModel>>> GetCompany(long companyId ,Search search)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from c in dc.GetSet<DsCompany>()
                         where c.Id == companyId || companyId == 0
                         select new CompanyModel
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Address = c.Address,
                             Phone = c.PhoneNumber,
                             Email = c.Email,
                             Manager = c.Manager,
                             CustomerNumber = (long)c.CustomerNumber
                         };

                qc = qc.Search(search, p => p.Name);

                return new GenericDataModel<IEnumerable<CompanyModel>>()
                {
                    Value = await qc.ToListAsync(),
                };
            }
        }

        public async Task<GenericDataModel<CompanyModel>> GetCompanyDetailed(long companyId, long selectedCompany)
        {
            using (var dc = _dcFactory.CreateContext())
            {
                var qc = from c in dc.GetSet<DsCompany>()
                         where (c.Id == companyId || companyId == 0) 
                         && selectedCompany == c.Id 
                         select new CompanyModel
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Address = c.Address,
                             Phone = c.PhoneNumber,
                             Email = c.Email,
                             Manager = c.Manager,
                             CustomerNumber = (long)c.CustomerNumber
                         };


                var company = await qc.FirstOrDefaultAsync();

                return new GenericDataModel<CompanyModel>()
                {
                    Value = company,
                };

            }
        }

        public async Task<CompanyModel> PatchCompanyAsync(JObject config, long companyId)
        {
            using (var dc = _dcFactory.CreateContext())
            {

                (dc as IHasChangeDetection)?.EnableChangeDetection();

                var company = await dc.GetSet<DsCompany>()
                                     .Where(q => q.Id == companyId)
                                     .FirstOrDefaultAsync();

                if (company == null)
                    throw new NotFoundException();

                JsonObjectGraph jog = new JsonObjectGraph(config, dc);

                await jog.ApplyToAsync(company, new DefaultContractResolver());

                var result = CompanyModel.FromDsCompany(company);
                if (result.Validate(out var message) == false)
                    throw new ValidationException(message);

                await dc.SaveChangesAsync();
                return result;
            }
        }

        public static async Task InternalAddCompany(IDataContext dc, CompanyModel companyInformation)
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
                    c.PhoneNumber = companyInformation.Phone;
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
            await ServiceRepository.InternalDeleteServicesForCompany(dc, companyId);

            // Delete all vessels
            foreach (var vessel in await ShipRepository.InternalGetVessels(dc, companyId, null))
            {
                await ShipRepository.InternalDeleteVesselAsync(dc, vessel.Id);
            }

            // Delete User Groups
            await UserGroupRepository.InternalDeleteUserGroupsForCompany(dc, companyId);

            var eq = dc as ICanExecuteQuery;

            var sql = @"DELETE FROM DS_COMPANY WHERE COM_COMPANYID = :cid";
            await eq.ExecuteSqlCommandAsync(sql, companyId);
        }

    }
}
