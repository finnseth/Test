using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Organization.Admin.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dualog.PortalService.Controllers.Organization.Admin
{
    [Authorize]
    [IsInDualog]
    [Route("api/v1")]
    public class AdminUserController : DualogController
    {

        AdminUserRepository _dbRepository;

        public AdminUserController(IDataContextFactory dcFactory)
            : base( dcFactory )
        {
            _dbRepository = new AdminUserRepository(dcFactory);
        }


        /// <summary>
        /// Get all admin users
        /// </summary>
        [ResourcePermission("OrganizationAdminUser", AccessRights.Read)]
        [HttpGet, Route("organization/admin/user")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(AdminUserModel), "The operation was successful.")]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _dbRepository.GetUser());
        }

        /// <summary>
        /// Get detailed info from one specific admin user
        /// </summary>
        [ResourcePermission("OrganizationAdminUser", AccessRights.Read)]
        [HttpGet, Route("organization/admin/user/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(AdminUserDetailModel), "The operation was successful.")]
        public async Task<IActionResult> GetDetailedUser(long id)
        {
            return Ok(await _dbRepository.GetDetailedUser(id));
        }


    }
}
