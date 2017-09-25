using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Organization.Shipping.Ship.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Ship
{
    [Authorize]
    [Route("api/v1/organization/shipping")]
    public class ShipController : DualogController
    {
        ShipRepository _dbRepository;

        public ShipController(IDataContextFactory dcFactory) : base(dcFactory)
        {
            _dbRepository = new ShipRepository(dcFactory);
        }


        /// <summary>
        /// Gets all ships setting for a given company for which the authorized user belongs to.
        /// </summary>
        [ResourcePermission("OrganizationShip", AccessRights.Read)]
        [HttpGet, Route("ship")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ShipModel), "The operation was successful.")]
        public Task<IActionResult> GetShip()
             => this.HandleGetAction(() => _dbRepository.GetShip(CompanyId));


        /// <summary>
        /// Gets all ships setting for a given company for which the authorized user belongs to.
        /// </summary>
        [ResourcePermission("OrganizationShip", AccessRights.Read)]
        [HttpGet, Route("shipwithquarantineinfo")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ShipModel), "The operation was successful.")]
        public Task<IActionResult> GetShipWithQuarantineInfo()
            => this.HandleGetAction(() => _dbRepository.GetShipWithQuarantineInfo(CompanyId));

        /// <summary>
        /// Gets the ship setting for a given ship for which the authorized user belongs to.
        /// </summary>
        [ResourcePermission("OrganizationShip", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/ship/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ShipModel), "The operation was successful.")]
        [IsInDualog]
        public Task<IActionResult> GetShipSetting(long id)
            => this.HandleGetAction(() => _dbRepository.GetShipAdmin(CompanyId, id));


        /// <summary>
        /// Adds a new vessel to the current logged in users company
        /// </summary>
        /// <param name="vessel">The vessel details.</param>
        /// <returns></returns>
        [HttpPost("ship")]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(ShipModel), "The created vessel.")]
        //[SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), "Something went wrong when adding the widget.")]
        public async Task<IActionResult> AddVessel([FromBody] ShipModel vessel)
        {
            try
            {
                await _dbRepository.AddVesselAsync(vessel, CompanyId);
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
