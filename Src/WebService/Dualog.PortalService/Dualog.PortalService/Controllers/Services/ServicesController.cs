using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Services.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dualog.PortalService.Controllers.Services
{
    /// <summary>
    /// The api's used to manage services.
    /// </summary>
    [Authorize]
    [IsInCompany]
    [Route( "api/v1" )]
    public class ServicesController : DualogController
    {
        ServicesRepository _repository;

        public ServicesController( IDataContextFactory dcFactory )
            : base( dcFactory )
        {
            _repository = new ServicesRepository( dcFactory );
        }

        /// <summary>
        /// Gets the service with the specified id.
        /// </summary>
        /// <param name="id">The id of the service to get.</param>
        /// <returns></returns>
        [HttpGet, Route( "internet/networkcontrol/services/{id}" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( ServiceDetails ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetSingleService( long id )
        {
            return this.HandleGetAction( () => _repository.GetServiceAsync( CompanyId, id ) );
        }

        /// <summary>
        /// Gets the services for company for which the current logged in user belongs to.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route( "internet/networkcontrol/services" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( ServiceDetails ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetServicesForCompany()
        {
            return this.HandleGetAction( () => _repository.GetServicesAsync( CompanyId, null ) );
        }


        /// <summary>
        /// Gets the services for the specified vessel.
        /// </summary>
        /// <param name="id">The id of the vessel to get the services for.</param>
        /// <returns></returns>
        [HttpGet, Route( "internet/networkcontrol/services/vessels/{id}" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( ServiceDetails ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetServicesForVessel( long id )
        {
            return this.HandleGetAction( () => _repository.GetServicesAsync( CompanyId, id ) );
        }

        /// <summary>
        /// Adds a new service to the specified vessel.
        /// </summary>
        /// <param name="vessel">The vessel to add the service to.</param>
        /// <param name="serviceDetails">The service details.</param>
        /// <returns></returns>
        [HttpPost, Route( "internet/networkcontrol/services/vessels/{vessel}" )]
        [SwaggerResponse( (int) HttpStatusCode.Created, typeof( ServiceDetails ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> AddServiceToVessel( long vessel, [FromBody] ServiceDetails serviceDetails )
        {
            return this.HandlePostAction( () => {
                if( serviceDetails.Validate( out var message ) == false )
                    throw new ValidationException( message );

                return  _repository.AddService( serviceDetails, CompanyId, vessel );
            } );
        }

        /// <summary>
        /// Adds a new service to the company which the current logged in user belongs to.
        /// </summary>
        /// <param name="vessel">The vessel to add the service to.</param>
        /// <param name="serviceDetails">The service details.</param>
        /// <returns></returns>
        [HttpPost, Route( "internet/networkcontrol/services" )]
        [SwaggerResponse( (int) HttpStatusCode.Created, typeof( ServiceDetails ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> AddServiceToCompany( long vessel, [FromBody] ServiceDetails serviceDetails )
        {
            return this.HandlePostAction( () => {
                if( serviceDetails.Validate( out var message ) == false )
                    throw new ValidationException( message );

                return _repository.AddService( serviceDetails, CompanyId );
            } );
        }

        /// <summary>
        /// Deletes the specified service.
        /// </summary>
        /// <param name="id">The id of the service to delete.</param>
        /// <returns></returns>
        [HttpDelete, Route( "internet/networkcontrol/services/{id}" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, Description = "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> DeleteService( long id )
        {
            return this.HandleDeleteAction( () => _repository.DeleteServiceAsync( CompanyId, id ) );
        }


        /// <summary>
        /// Patches the specified service.
        /// </summary>
        /// <param name="id">The id of the service to patch.</param>
        /// <param name="json">The json document containing the changes.</param>
        /// <returns></returns>
        [HttpPatch, Route( "internet/networkcontrol/services/{id}" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( ServiceDetails ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> PatchService( long id, [FromBody] JObject json )
        {
            return this.HandlePatchAction( () => _repository.PatchService( CompanyId, id, json ) );
        }
    }
}
