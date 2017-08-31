using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dualog.PortalService.Controllers.Companies.Model;
using System.Net;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Dualog.PortalService.Core;
using Dualog.Data.Entity;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Dualog.PortalService.Controllers.Companies
{
    [Authorize]
    [IsInDualog]
    [Route( "api/v1" )]
    public class CompanyController : DualogController
    {
        CompanyRepository _companyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyController"/> class.
        /// </summary>
        public CompanyController( IDataContextFactory dcFactory )
            : base( dcFactory )
        {
            _companyRepository = new CompanyRepository(DataContextFactory);
        }

        /// <summary>
        /// Returns all the companies for the Dualog users.
        /// </summary>
        /// <response code="200">Ok</response>
        [HttpGet, Route( "companies" )]
        //[SwaggerResponse( HttpStatusCode.OK, Type = typeof( Company ) )]
        public async Task<IActionResult> All()
        {
            return  Ok(await _companyRepository.GetCompanies( ) );
        }
    }
}
