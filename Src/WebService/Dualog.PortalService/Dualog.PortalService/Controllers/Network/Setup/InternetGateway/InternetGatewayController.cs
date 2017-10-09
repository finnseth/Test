using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Network.Setup.InternetGateway.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dualog.PortalService.Controllers.Network.Setup.InternetGateway

{
    [Authorize]
    [IsInCompany]
    [Route( "api/v1/network/setup/internetgateway/" )]
    public class CarrierTypesController : DualogController
    {
        InternetGatewayRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarrierTypeModel"/> class.
        /// </summary>
        public CarrierTypesController( IDataContextFactory factory )
            : base( factory )
        {
            _repository = new InternetGatewayRepository( factory );
        }

        /// <summary>
        /// Gets the methods
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route( "gatewaytype" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( CarrierTypeModel[] ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetMethods() =>
            this.HandleGetAction( () =>
                _repository.GetMethods() );

        [HttpGet, Route("ship/{shipId}/gateway")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(CommunicationMethodModel[]), "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public Task<IActionResult> GetAllComMethods(long shipId) =>
            this.HandleGetAction(() =>
                _repository.GetCommMethods(CompanyId, shipId));


        [HttpGet, Route("ship/{shipId}/gateway/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(CommunicationMethodDetailsModel), "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public Task<IActionResult> GetSingleComMethod(long shipId, long id) =>
            this.HandleGetAction(() =>
                _repository.GetSingleComMethod(CompanyId, shipId, id));

    }
}
