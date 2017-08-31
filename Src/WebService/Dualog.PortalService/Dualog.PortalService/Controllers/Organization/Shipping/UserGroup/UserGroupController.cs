using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers.Organization.Shipping.UserGroup
{
    [Authorize]
    [Route("api/v1")]
    public class UserGroupController : DualogController
    {

        UserGroupRepository _dbRepository;

        public UserGroupController(IDataContextFactory dcFactory) : base(dcFactory)
        {
            _dbRepository = new UserGroupRepository(dcFactory);
        }

        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/usergroup")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserGroupModel), "The operation was successful.")]
        public async Task<IActionResult> GetUserGroup(long id)
        {
            return Ok(await _dbRepository.GetUserGroup(CompanyId));
        }


        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/usergroup/shipgroup/{shipid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserGroupModel), "The operation was successful.")]
        public async Task<IActionResult> GetShipUserGroup(long shipid)
        {
            return Ok(await _dbRepository.GetShipUserGroup(CompanyId, shipid));
        }

        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/usergroup/forship/{shipid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserGroupModel), "The operation was successful.")]
        public async Task<IActionResult> GetUserGroupForShip(long shipid)
        {
            return Ok(await _dbRepository.GetUserGroupForShip(CompanyId, shipid));
        }

        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/usergroup/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserGroupDetailModel), "The operation was successful.")]
        public async Task<IActionResult> GetUserGroupDetail(long id)
        {
            return Ok(await _dbRepository.GetUserGroupDetail(CompanyId, id));
        }


    }
}
