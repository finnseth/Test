using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.CommunicationMethods.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dualog.PortalService.Controllers.CommunicationMethods
{
    [Authorize]
    [IsInCompany]
    [Route( "api/v1" )]
    public class CommunicationMethodsController : DualogController
    {
        CommunicationMethodsRepository _repository;

        public CommunicationMethodsController( IDataContextFactory factory ) 
            : base( factory )
        {
            _repository = new CommunicationMethodsRepository( factory );
        }

        [HttpGet, Route( "vessels/{vesselId}/comMethods" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( CommunicationMethodModel[] ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetAllComMethods( long vesselId ) => this.HandleGetAction( () => _repository.GetCommMethods(CompanyId, vesselId) );


        [HttpGet, Route( "vessels/{vesselId}/comMethods/{id}" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( CommunicationMethodDetailsModel ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public Task<IActionResult> GetSingleComMethod( long vesselId, long id ) => this.HandleGetAction( () => _repository.GetSingleComMethod(CompanyId, vesselId, id) );
    }
}
