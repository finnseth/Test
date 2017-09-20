using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Users.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dualog.PortalService.Controllers.Users
{
    /// <summary>
    /// Defines endpoints to work with User resources
    /// </summary>
    [Authorize]
    [IsInCompany]
    [Route("api/v1")]
    public class UsersController : DualogController
    {
        private readonly UserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        public UsersController(IDataContextFactory dcFactory)
            : base( dcFactory )
        {
            _userRepository = new UserRepository(dcFactory);
        }


        /// <summary>
        /// Gets all users for the company for which the authorized user belongs to.
        /// </summary>
        [HttpGet, Route( "users" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( UserSummaryModel ), "The operation was successful." )]
        public Task<IActionResult> AllAsync( 
            [FromQuery] bool includeTotalCount = false ) 
                => this.HandleGetAction( () => _userRepository.GetUsersAsync( CompanyId, HttpContext.Pagination(), HttpContext.Search(), includeTotalCount ) );


        /// <summary>
        /// Gets the details of the user with the given id.
        /// </summary>
        /// <param name="id">The id of the user to get.</param>
        /// <returns></returns>
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( UserDetailsModel ), "The operation was successful." )]
        [HttpGet, Route( "users/{id}" )]
        public Task<IActionResult> GetUserById( long id ) 
            => this.HandleGetAction( () => _userRepository.GetUserDetailsAsync( id, CompanyId ) );


        /// <summary>
        /// Adds a new user to the current authorized users company.
        /// </summary>
        /// <param name="user">The user information.</param>
        /// <returns></returns>
        [HttpPost, Route("users")]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(UserGroupModel), "The operation was successful.")]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        public async Task<IActionResult> AddUser( [FromBody] UserDetailsModel user)
        {
            try
            {
                if (user.Validate(out var result) == false)
                    return new BadRequestObjectResult(new ErrorObject { Message = result });

                await _userRepository.CreateUser(user, CompanyId);

                return Created( "", user );
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
        [HttpDelete, Route( "users/{id}" )]
        [SwaggerResponse( (int) HttpStatusCode.OK, Description = "The operation was successful." )]
        public Task<IActionResult> DeleteUser( long id ) 
            => this.HandleDeleteAction( () => _userRepository.DeleteUser( id, CompanyId ) );

        /// <summary>
        /// Patch the user with the given id.
        /// </summary>
        /// <param name="id">The id of the user to patch</param>
        /// <param name="json">The patch document.</param>
        /// <returns></returns>
        /// <remarks>
        /// The name of function is used as lookup on the various patch operators instead of the permissions id.
        /// </remarks>
        [HttpPatch, Route( "users/{id}" )]
        [SwaggerOperationFilter( typeof( UserDetailsModel ) )]
        [SwaggerResponse( (int) HttpStatusCode.OK, typeof( UserDetailsModel ), "The operation was successful." )]
        [SwaggerResponse( (int) HttpStatusCode.InternalServerError )]
        [SwaggerResponse( (int) HttpStatusCode.NotFound )]
        [SwaggerResponse( (int) HttpStatusCode.BadRequest, typeof( ErrorObject ), Description = "The operation failed because of an invalid patch document." )]
        public Task<IActionResult> PatchUser( long id, [FromBody] JObject json ) 
            => this.HandlePatchAction( () => _userRepository.PatchUserAsync( json, id ) );

    }
}
