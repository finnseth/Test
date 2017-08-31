using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Organization.Shipping.User.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers.Organization.Shipping.User
{
    [Authorize]
    [Route("api/v1")]
    public class UserController : DualogController
    {

        UserRepository _dbRepository;

        public UserController(IDataContextFactory dcFactory) : base(dcFactory)
        {
            _dbRepository = new UserRepository(dcFactory);
        }


        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/user")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserModel), "The operation was successful.")]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _dbRepository.GetUser(CompanyId));
        }

        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/user/shipuser/{shipid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserModel), "The operation was successful.")]
        public async Task<IActionResult> GetUser(long shipid)
        {
            return Ok(await _dbRepository.GetShipUser(CompanyId, shipid));
        }


        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/user/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserDetailModel), "The operation was successful.")]
        public async Task<IActionResult> GetUserDetail(long id)
        {
            return Ok(await _dbRepository.GetUserDetail(CompanyId, id));
        }


    }
}
