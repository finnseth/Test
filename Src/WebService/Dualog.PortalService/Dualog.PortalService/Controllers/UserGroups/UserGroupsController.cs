using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.UserGroups.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Dualog.PortalService.Core;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers.UserGroups
{
    [Authorize]
    [IsInCompany]
    [Route( "api/v1" )]
    public class UserGroupsController : DualogController
    {
        private readonly UserGroupsRepository _userGroupRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserGroupsController"/> class.
        /// </summary>
        public UserGroupsController( IDataContextFactory dcFactory )
            : base( dcFactory )
        {
            _userGroupRepository = new UserGroupsRepository( dcFactory );
        }

        /// <summary>
        /// Returns all the user groups for the current users company.        
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( UserGroupDetails ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError, Description = "Internal server error" )]
        [HttpGet, Route( "userGroups" )]
        public async Task<IActionResult> GetAllForCompany()
        {
            try
            {
                return Ok( await _userGroupRepository.GetUserGroups( CompanyId ) );
            }
            catch( Exception exception )
            {
                Log.Error( "{Url} {Verb} failed: {Exception}", exception );
                return new StatusCodeResult( (int) HttpStatusCode.InternalServerError );
            }
        }

        /// <summary>
        /// Gets the user group with the specified id.        
        /// </summary>
        /// <param name="id">The id of the user group to get.</param>
        /// <returns></returns>
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( UserGroupDetails ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.NotFound )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError, Description = "Internal server error" )]
        [HttpGet, Route( "userGroups/{id}" )]
        public async Task<IActionResult> GetSingle( long id )
        {
            try
            {
                UserGroupDetails userGroupDetails = (await _userGroupRepository.GetUserGroups( CompanyId, null, id )).FirstOrDefault();
                if( userGroupDetails == null )
                    return NotFound();

                return Ok( userGroupDetails );
            }
            catch( Exception exception )
            {
                Log.Error( "{Url} {Verb} failed: {Exception}", exception );
                return new StatusCodeResult( (int) HttpStatusCode.InternalServerError );
            }
        }



        /// <summary>
        /// Creates a new user group for the specified company.
        /// </summary>
        /// <param name="userGroup">The user group details.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [SwaggerResponse( (int) HttpStatusCode.Created, typeof( UserGroupDetails ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.BadRequest, typeof( ErrorObject ), "There was an error with the request." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError, Description = "Internal server error" )]
        [HttpPost, Route( "userGroups" )]
        public async Task<IActionResult> CreateUserGroupForCompany( [FromBody] UserGroupDetails userGroup )
        {
            try
            {
                if( userGroup.Validate( out var message ) == false )
                    throw new ValidationException( message );

                await _userGroupRepository.AddUserGroup( userGroup, CompanyId );
                return Created( "", userGroup );
            }

            catch( ValidationException exception )
            {
                Log.Error( "{Url} {Verb} failed: {Exception}", exception );
                return new BadRequestObjectResult( new ErrorObject { Message = exception.Message } );

            }

            catch( Exception exception )
            {
                Log.Error( "{Url} {Verb} failed: {Exception}", exception );
                return new StatusCodeResult( (int) HttpStatusCode.InternalServerError );
            }
        }


        //[SwaggerResponse( (int) HttpStatusCode.Created, typeof( UserGroupDetails ), "The operation was successful." )]
        //[SwaggerResponse( (int) HttpStatusCode.InternalServerError, Description = "Internal server error" )]
        //[HttpPost, Route( "usersGroups/vessels/{id}" )]
        //public async Task<IActionResult> CreateUserGroupForVessel( long id, [FromBody] UserGroupDetails userGroup )
        //{
        //    try
        //    {
        //        return Created( "", null );
        //    }
        //    catch( Exception exception )
        //    {
        //        Log.Error( "{Url} {Verb} failed: {Exception}", exception );
        //        return new StatusCodeResult( (int) HttpStatusCode.InternalServerError );
        //    }
        //}


        /// <summary>
        /// Patch the user group with the given id.
        /// </summary>
        /// <param name="id">The id of the user group to be patched.</param>
        /// <param name="patch">The patch document.</param>
        /// <returns></returns>
        /// <remarks>
        /// The name of function is used as lookup on the various patch operators instead of the permissions id.
        /// </remarks>
        [SwaggerOperationFilter(typeof( UserGroupDetails ) )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( UserGroupDetails ), Description = "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.BadRequest, Type = typeof(ErrorObject), Description = "The request was invalid." )]
        [SwaggerResponse( (int) HttpStatusCode.NotFound, Description = "The object to patch was not found." )]
        [HttpPatch, Route( "userGroups/{id}" )]
        public async Task<IActionResult> PatchUserGroup( long id, [FromBody] JObject patch )
        {
            try
            {
                var  userGroup = await _userGroupRepository.ChangeUserGroupAsync( id, patch );
                return Ok( userGroup );
            }

            catch( ValidationException exception )
            {
                Log.Error( "{Url} {Verb} failed: {Exception}", exception );
                return BadRequest( new ErrorObject { Message = exception.Message } );
            }

            catch( NotFoundException exception )
            {
                Log.Error( "{Url} {Verb} failed: {Exception}", exception );
                return NotFound();
            }

            catch( Exception exception )
            {
                Log.Error( "{Url} {Verb} failed: {Exception}", exception );
                return new StatusCodeResult( (int) HttpStatusCode.InternalServerError );
            }
        }

        /// <summary>
        /// Deletes the user group with the specified id
        /// </summary>
        /// <param name="id">The id of the user group to delete.</param>
        /// <returns></returns>
        [SwaggerResponse( (int) HttpStatusCode.Created, Description = "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError, Description = "Internal server error" )]
        [HttpDelete, Route( "userGroups/{id}" )]
        public async Task<IActionResult> DeleteUserGroup( long id )
        {
            try
            {
                await _userGroupRepository.DeleteUserGroup( CompanyId, id );
                return Ok();
            }
            catch( Exception exception )
            {
                Log.Error( "{Url} {Verb} failed: {Exception}", exception );
                return new StatusCodeResult( (int) HttpStatusCode.InternalServerError );
            }
        }
    }
}
