using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Methods.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dualog.PortalService.Controllers.Methods
{
    [Authorize]
    [IsInCompany]
    [Route( "api/v1" )]
    public class CarrierTypesController : DualogController
    {
        CarrierTypesRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarrierTypesController"/> class.
        /// </summary>
        public CarrierTypesController( IDataContextFactory factory )
            : base( factory )
        {
            _repository = new CarrierTypesRepository( factory );
        }

        /// <summary>
        /// Gets the methods
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route( "core/carrierType" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( CarrierTypeDetails[] ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetMethods() => this.HandleGetAction( () => _repository.GetMethods() );
    }
}
