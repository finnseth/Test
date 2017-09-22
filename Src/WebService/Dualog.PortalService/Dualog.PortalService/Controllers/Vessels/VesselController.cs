using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dualog.PortalService.Controllers.Vessels.Model;
using System.Net;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Dualog.PortalService.Core;
using Dualog.Data.Entity;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Dualog.PortalService.Controllers.Vessels
{
    [Authorize]
    [IsInCompany]
    [Route("api/v1")]
    public class VesselController : DualogController
    {
        VesselRepository _vesselRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselController"/> class.
        /// </summary>
        public VesselController(IDataContextFactory dcFactory)
            : base(dcFactory)
        {
            _vesselRepository = new VesselRepository(DataContextFactory);
        }


        [HttpGet, Route("vessels/{id}")]
        public async Task<IActionResult> Single(long id)
        {
            var qry = from v in await _vesselRepository.GetVessels(CompanyId, null)
                      where v.Id == id
                      select v;


            return Ok(qry);
        }


        /// <summary>
        /// Returns all the vessel for the current users company.
        /// </summary>
        /// <response code="200">Ok</response>
        [HttpGet, Route("vessels")]
        //[SwaggerResponse( HttpStatusCode.OK, Type = typeof( Vessel ) )]
        public async Task<IActionResult> All()
        {
            return Ok(await _vesselRepository.GetVessels(CompanyId, HttpContext.Pagination(), HttpContext.Search()));
        }


        /// <summary>
        /// Adds a new vessel to the current logged in users company
        /// </summary>
        /// <param name="vessel">The vessel details.</param>
        /// <returns></returns>
        [HttpPost("vessels")]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(VesselDetails), "The created vessel.")]
        //[SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), "Something went wrong when adding the widget.")]
        public async Task<IActionResult> AddVessel([FromBody] VesselDetails vessel)
        {
            try
            {
                await _vesselRepository.AddVesselAsync(vessel, CompanyId);
                return Created($"{Request.Host}/api/v1/vessels/{vessel.Id}", vessel);
            }
            catch (Exception exception)
            {
                Log.Error("AddVessel failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
