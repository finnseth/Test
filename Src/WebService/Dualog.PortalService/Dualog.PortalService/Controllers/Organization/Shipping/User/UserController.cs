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
using Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model;

namespace Dualog.PortalService.Controllers.Organization.Shipping.User
{
    [Authorize]
    [Route("api/v1/organization/shipping/")]
    public class UserController : DualogController
    {

        UserRepository _dbRepository;

        public UserController(IDataContextFactory dcFactory) : base(dcFactory)
        {
            _dbRepository = new UserRepository(dcFactory);
        }



        /// <summary>
        /// Gets all users for the company for which the authorized user belongs to.
        /// </summary>
        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("user")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserModel), "The operation was successful.")]
        public Task<IActionResult> AllAsync([FromQuery] bool includeTotalCount = false)
                => this.HandleGetAction(() => _dbRepository.GetUserAsync(CompanyId, HttpContext.Pagination(), HttpContext.Search(), includeTotalCount));



        /// <summary>
        /// Gets all users for a specific ship.
        /// </summary>
        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("shipuser/{shipid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserModel), "The operation was successful.")]
        public Task<IActionResult> GetShipUser(long shipid)
            => this.HandleGetAction(() => _dbRepository.GetShipUser(CompanyId, shipid));


        /// <summary>
        /// Gets detailed information about a specific user
        /// </summary>
        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("user/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserDetailModel), "The operation was successful.")]
        public Task<IActionResult> GetUserDetail(long id)
            => this.HandleGetAction(() => _dbRepository.GetUserDetailsAsync(id, CompanyId));


        /// <summary>
        /// Adds a new user to the current authorized users company.
        /// </summary>
        /// <param name="user">The user information.</param>
        /// <returns></returns>
        [HttpPost, Route("user")]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(UserGroupModel), "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddUser([FromBody] UserDetailModel user)
        {
            try
            {
                if (user.Validate(out var result) == false)
                    return new BadRequestObjectResult(new ErrorObject { Message = result });

                await _dbRepository.CreateUser(user, CompanyId);

                return Created("", user);
            }
            catch (Exception exception)
            {
                Log.Error("{Url} {Verb} failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Deletes the user with the specified id.
        /// </summary>
        /// <param name="id">The id of the user to delete.</param>
        /// <returns></returns>
        [HttpDelete, Route("user/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "The operation was successful.")]
        public Task<IActionResult> DeleteUser(long id)
            => this.HandleDeleteAction(() => _dbRepository.DeleteUser(id, CompanyId));


        /// <summary>
        /// Patch the user with the given id.
        /// </summary>
        /// <param name="id">The id of the user to patch</param>
        /// <param name="json">The patch document.</param>
        /// <returns></returns>
        /// <remarks>
        /// The name of function is used as lookup on the various patch operators instead of the permissions id.
        /// </remarks>
        [HttpPatch, Route("user/{id}")]
        [SwaggerOperationFilter(typeof(UserDetailModel))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserDetailModel), "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), Description = "The operation failed because of an invalid patch document.")]
        public Task<IActionResult> PatchUser(long id, [FromBody] JObject json)
            => this.HandlePatchAction(() => _dbRepository.PatchUserAsync(json, id));


    }
}
