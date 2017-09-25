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
    [Route("api/v1/organization/shipping")]
    public class UserGroupController : DualogController
    {

        UserGroupRepository _dbRepository;

        public UserGroupController(IDataContextFactory dcFactory) : base(dcFactory)
        {
            _dbRepository = new UserGroupRepository(dcFactory);
        }

        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("usergroup")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserGroupModel), "The operation was successful.")]
        public async Task<IActionResult> GetUserGroup(long id)
        {
            return Ok(await _dbRepository.GetUserGroup(CompanyId));
        }


        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("usergroup/shipgroup/{shipid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserGroupModel), "The operation was successful.")]
        public async Task<IActionResult> GetShipUserGroup(long shipid)
        {
            return Ok(await _dbRepository.GetShipUserGroup(CompanyId, shipid));
        }

        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("usergroup/forship/{shipid}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserGroupModel), "The operation was successful.")]
        public async Task<IActionResult> GetUserGroupForShip(long shipid)
        {
            return Ok(await _dbRepository.GetUserGroupForShip(CompanyId, shipid));
        }

        [ResourcePermission("OrganizationUser", AccessRights.Read)]
        [HttpGet, Route("usergroup/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserGroupDetailModel), "The operation was successful.")]
        public async Task<IActionResult> GetUserGroupDetail(long id)
        {
            return Ok(await _dbRepository.GetUserGroupDetail(CompanyId, id));
        }

        /// <summary>
        /// Creates a new user group for the specified company.
        /// </summary>
        /// <param name="userGroup">The user group details.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(UserGroupDetailModel), "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), "There was an error with the request.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Internal server error")]
        [HttpPost, Route("usergroup")]
        public async Task<IActionResult> CreateUserGroupForCompany([FromBody] UserGroupDetailModel userGroup)
        {
            try
            {
                if (userGroup.Validate(out var message) == false)
                    throw new ValidationException(message);

                await _dbRepository.AddUserGroup(userGroup, CompanyId);
                return Created("", userGroup);
            }

            catch (ValidationException exception)
            {
                Log.Error("{Url} {Verb} failed: {Exception}", exception);
                return new BadRequestObjectResult(new ErrorObject { Message = exception.Message });

            }

            catch (Exception exception)
            {
                Log.Error("{Url} {Verb} failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Patch the user group with the given id.
        /// </summary>
        /// <param name="id">The id of the user group to be patched.</param>
        /// <param name="patch">The patch document.</param>
        /// <returns></returns>
        /// <remarks>
        /// The name of function is used as lookup on the various patch operators instead of the permissions id.
        /// </remarks>
        [SwaggerOperationFilter(typeof(UserGroupDetailModel))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserGroupDetailModel), Description = "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(ErrorObject), Description = "The request was invalid.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "The object to patch was not found.")]
        [HttpPatch, Route("usergroup/{id}")]
        public async Task<IActionResult> PatchUserGroup(long id, [FromBody] JObject patch)
        {
            try
            {
                var userGroup = await _dbRepository.ChangeUserGroupAsync(id, patch);
                return Ok(userGroup);
            }

            catch (ValidationException exception)
            {
                Log.Error("{Url} {Verb} failed: {Exception}", exception);
                return BadRequest(new ErrorObject { Message = exception.Message });
            }

            catch (NotFoundException exception)
            {
                Log.Error("{Url} {Verb} failed: {Exception}", exception);
                return NotFound();
            }

            catch (Exception exception)
            {
                Log.Error("{Url} {Verb} failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Deletes the user group with the specified id
        /// </summary>
        /// <param name="id">The id of the user group to delete.</param>
        /// <returns></returns>
        [SwaggerResponse((int)HttpStatusCode.Created, Description = "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Internal server error")]
        [HttpDelete, Route("usergroup/{id}")]
        public async Task<IActionResult> DeleteUserGroup(long id)
        {
            try
            {
                await _dbRepository.DeleteUserGroup(CompanyId, id);
                return Ok();
            }
            catch (Exception exception)
            {
                Log.Error("{Url} {Verb} failed: {Exception}", exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


    }
}
