﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.NetworkControlRules.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dualog.PortalService.Controllers.NetworkControlRules
{
    [Authorize]
    [IsInCompany]
    [Route( "api/v1" )]
    public class NetworkControlRulesController : DualogController
    {
        NetworkControlRulesRepository _repository;

        public NetworkControlRulesController( IDataContextFactory dcFactory )
            : base( dcFactory )
        {
            _repository = new NetworkControlRulesRepository( dcFactory );
        }

        /// <summary>
        /// Gets the computer rights for the company for which the current user is a member of.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route( "internet/networkcontrol/rules" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( CompanyRule ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetRightsForCompany()
        {
            return this.HandleGetAction( () => _repository.GetRulesForCompany( CompanyId ) );
        }

        /// <summary>
        /// Adds a new network control rule to the company.
        /// </summary>
        /// <param name="companyRule">The rule details to be added.</param>
        /// <returns></returns>
        [HttpPost, Route( "internet/networkcontrol/rules" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( CompanyRule ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> AddRightsForCompany( [FromBody] CompanyRule companyRule )
        {
            return this.HandlePostAction( () => _repository.AddRightsForCompanyAsync( CompanyId, companyRule ) );
        }

        /// <summary>
        /// Gets the computer rights for the specified vessel.
        /// </summary>
        /// <param name="id">The id of the vessel for which to get the rights.</param>
        /// <returns></returns>
        [HttpGet, Route( "internet/networkcontrol/rules/vessels/{id}" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( CompanyRule ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetRightsForVessel( long id )
        {
            return this.HandleGetAction( () => _repository.GetRulesForVessel( CompanyId, id ) );
        }
    }
}
