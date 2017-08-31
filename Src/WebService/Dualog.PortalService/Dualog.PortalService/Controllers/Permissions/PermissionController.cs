using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Permissions.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dualog.PortalService.Controllers.Permissions
{
    [Authorize]
    [Route("api/v1")]
    public class PermissionController : DualogController
    {
        PermissionRepository _dbRepository;


        public PermissionController(IDataContextFactory dcFactory)
            : base( dcFactory )
        {
            _dbRepository = new PermissionRepository(dcFactory);
        }


        /// <summary>
        /// Gets all permissions for the current logged in user
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(PermissionDetails), "The operation was successful.")]
        [HttpGet, Route("permissions")]
        public async Task<IActionResult> GetAllPermissions()
        {
            IEnumerable<PermissionDetails> result = null;
            if (IsDualogAdmin == false)
                result = await _dbRepository.GetAllPermission(UserId);
            else
                result = await _dbRepository.GetAdminPermissions(UserId);


            return Ok(  result );
        }

    }
}
