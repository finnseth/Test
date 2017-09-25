using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Email.Setup.Domain.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dualog.PortalService.Controllers.Email.Setup.Domain
{

    /// <summary>
    /// Defines endpoints to work with User resources
    /// </summary>
    [Authorize]
    [Route("api/v1")]

    public class DomainController: DualogController
    {

        private readonly DomainRepository _domainRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainController"/> class.
        /// </summary>
        public DomainController(IDataContextFactory dcFactory)
            : base( dcFactory )
        {
            _domainRepository = new DomainRepository(dcFactory);
        }


        /// <summary>
        /// Gets all users for the company for which the authorized user belongs to.
        /// </summary>
        [HttpGet, Route("email/setup/domain")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(DomainModel), "The operation was successful.")]
        public Task<IActionResult> AllAsync(
            [FromQuery] bool includeTotalCount = false)
                => this.HandleGetAction(() => _domainRepository.GetDomainAsync(CompanyId, HttpContext.Pagination(), includeTotalCount));

    }
}
