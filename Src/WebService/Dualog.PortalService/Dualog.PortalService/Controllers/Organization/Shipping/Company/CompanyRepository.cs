using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Organization.Shipping.Company.Model;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;


using System.Data;
using Dualog.PortalService.Controllers.Organization.Admin.Model;
using Dualog.Data.Oracle.Entity;




namespace Dualog.PortalService.Controllers.Organization.Shipping.Company
{
    public class CompanyRepository
    {

        IDataContextFactory _dcFactory;

        public CompanyRepository(IDataContextFactory dcFactory)
        {
            _dcFactory = dcFactory;
        }

        public async Task<IEnumerable<CompanyModel>> GetCompany(long companyId)
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

                var company = await qc.ToListAsync();

                return company;
            }
        }

        public async Task<CompanyModel> GetCompanyDetailed(long companyId, long selectedCompany)
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

                return company;
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
    }
}
