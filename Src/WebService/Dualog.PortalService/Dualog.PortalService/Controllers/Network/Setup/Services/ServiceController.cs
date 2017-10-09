using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Network.Setup.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dualog.PortalService.Controllers.Network.Setup.Services
{
    /// <summary>
    /// The api's used to manage services.
    /// </summary>
    [Authorize]
    [Route( "api/v1/network/setup" )]
    public class ServiceController : DualogController
    {
        ServiceRepository _repository;

        public ServiceController( IDataContextFactory dcFactory )
            : base( dcFactory )
        {
            _repository = new ServiceRepository( dcFactory );
        }

        /// <summary>
        /// Gets the service with the specified id.
        /// </summary>
        /// <param name="id">The id of the service to get.</param>
        /// <returns></returns>
        [HttpGet, Route( "service/{id}" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( ServiceDetailModel ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetSingleService( long id ) =>
            this.HandleGetAction( () =>
                _repository.GetServiceAsync( CompanyId, id ));


        /// <summary>
        /// Gets the services for company for which the current logged in user belongs to.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route( "service" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( ServiceDetailModel ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetServicesForCompany() =>
            this.HandleGetAction( () =>
                _repository.GetServicesAsync( CompanyId, null ));


        /// <summary>
        /// Gets the services for the specified vessel.
        /// </summary>
        /// <param name="shipId">The id of the vessel to get the services for.</param>
        /// <returns></returns>
        [HttpGet, Route( "shipservice/{shipId}" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( ServiceDetailModel ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetServicesForVessel( long shipId) =>
            this.HandleGetAction( () =>
                _repository.GetServicesAsync( CompanyId, shipId) );

        /// <summary>
        /// Adds a new service to the specified vessel.
        /// </summary>
        /// <param name="shipId">The vessel to add the service to.</param>
        /// <param name="serviceDetails">The service details.</param>
        /// <returns></returns>
        [HttpPost, Route( "shipservice/{shipId}" )]
        [SwaggerResponse( (int) HttpStatusCode.Created, typeof( ServiceDetailModel ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> AddServiceToVessel( long shipId, [FromBody] ServiceDetailModel serviceDetails )
        {
            return this.HandlePostAction( () => {
                if( serviceDetails.Validate( out var message ) == false )
                    throw new ValidationException( message );

                return  _repository.AddService( serviceDetails, CompanyId, shipId );
            } );
        }

        /// <summary>
        /// Adds a new service to the company which the current logged in user belongs to.
        /// </summary>
        /// <param name="serviceDetails">The service details.</param>
        /// <returns></returns>
        [HttpPost, Route( "companyservice" )]
        [SwaggerResponse( (int) HttpStatusCode.Created, typeof( ServiceDetailModel ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> AddServiceToCompany( [FromBody] ServiceDetailModel serviceDetails )
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
        [HttpDelete, Route("service/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public Task<IActionResult> DeleteService(long id) =>
            this.HandleDeleteAction(() =>
                _repository.DeleteServiceAsync(CompanyId, id));


        /// <summary>
        /// Patches the specified service.
        /// </summary>
        /// <param name="id">The id of the service to patch.</param>
        /// <param name="json">The json document containing the changes.</param>
        /// <returns></returns>
        [HttpPatch, Route("service/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ServiceDetailModel), "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public Task<IActionResult> PatchService(long id, [FromBody] JObject json) =>
            this.HandlePatchAction(() =>
                _repository.PatchService(CompanyId, id, json));
    }
}
