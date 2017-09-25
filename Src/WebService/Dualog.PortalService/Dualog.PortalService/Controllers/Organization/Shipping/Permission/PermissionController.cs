using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Organization.Shipping.Permission.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Dualog.PortalService.Models;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Permission
{
    [Authorize]
    [Route("api/v1/organization/shipping")]
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
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(PermissionDetailModel), "The operation was successful.")]
        [HttpGet, Route("permission")]
        public async Task<IActionResult> GetAllPermission()
        {
            //           IEnumerable<PermissionDetailModel> result = null;
            GenericDataModel<IEnumerable<PermissionDetailModel>> result = null;
            if (IsDualogAdmin == false)
                result = await _dbRepository.GetAllPermission(UserId);
            else
                result = await _dbRepository.GetAdminPermission(UserId);


            return Ok(  result );
        }

    }
}
