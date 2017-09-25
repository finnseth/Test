using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Email.Setup.Domain.Model;
using Dualog.PortalService.Core;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Email.Setup.Domain
{
    public class DomainRepository
    {
        readonly IDataContextFactory _db;

        public DomainRepository(IDataContextFactory dcFactory)
        {
            _db = dcFactory;
        }

        /// <summary>
        /// Get all users for a given company
        /// </summary>
        /// <param name="companyId">The id of the company.</param>
        /// <param name="pagination">The pagination.</param>
        /// <param name="includeTotalCount">if set to <c>true</c> [include total count].</param>
        /// <returns></returns>
        public Task<GenericDataModel<IEnumerable<DomainModel>>> GetDomainAsync(long companyId, Pagination pagination, bool includeTotalCount = false)
            => _db.CreateContext().Use(async dc =>
            {
                var query = from c in dc.GetSet<DsCompany>()
                            where c.Id == companyId || companyId == 0
                            orderby c.Name ascending
                            select new DomainModel
                            {
                                Id = c.Id,
                                DomainPrefix = c.DomainPrefix,
                                CustomDomains = from cd in c.CustomDomains
                                                select new CustomDomainModel
                                                {
                                                    Id = cd.Id,
                                                    Domain = cd.CustomDomain
                                                },

                            };

                return new GenericDataModel<IEnumerable<DomainModel>>()
                {
                    Value = await query.Paginate(pagination).ToListAsync(),
                    TotalCount = includeTotalCount ? await query.CountAsync() : 0
                };
            });



    }
}