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
    [Route("api/v1")]
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
        [HttpGet, Route("organization/shipping/ship")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ShipModel), "The operation was successful.")]
        public async Task<IActionResult> GetShip()
        {
            return Ok(await _dbRepository.GetShip(CompanyId));
        }

        /// <summary>
        /// Gets all ships setting for a given company for which the authorized user belongs to.
        /// </summary>
        [ResourcePermission("OrganizationShip", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/shipwithquarantineinfo")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ShipModel), "The operation was successful.")]
        public async Task<IActionResult> GetShipWithQuarantineInfo()
        {
            return Ok(await _dbRepository.GetShipWithQuarantineInfo(CompanyId));
        }

        /// <summary>
        /// Gets the ship setting for a given ship for which the authorized user belongs to.
        /// </summary>
        [ResourcePermission("OrganizationShip", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/ship/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ShipModel), "The operation was successful.")]
        [IsInDualog]
        public async Task<IActionResult> GetShipSetting(long id)
        {
            return Ok(await _dbRepository.GetShipAdmin(CompanyId, id));
        }
    }
}
