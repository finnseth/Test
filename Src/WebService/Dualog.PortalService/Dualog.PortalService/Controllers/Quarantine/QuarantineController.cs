using System;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Quarantine.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers.Quarantine
{
    [Authorize]
    [IsInCompany]
    [Route("api/v1")]
    public class QuarantineController : DualogController
    {
        private QuarantineRepository _dbRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="QuarantineController"/> class.
        /// </summary>
        /// <param name="identityData">The identity data.</param>
        /// <param name="dcFactory">The dc factory.</param>
        public QuarantineController(IDataContextFactory dcFactory)
            : base( dcFactory )
        {
            _dbRepository = new QuarantineRepository(dcFactory);
        }


        /// <summary>
        /// Gets the company configuration for quarantine.
        /// </summary>
        /// <returns></returns>
        [ResourcePermission("EmailRestriction", AccessRights.Read)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(QuarantineCompanyConfig), "The operation was successful.")]
        [HttpGet, Route("email/settings/quarantine")]
        public async Task<IActionResult> GetCompanyConfig()
        {
            try
            {
                var result = await _dbRepository.GetCompanyConfig(CompanyId);
                return base.Ok(result);

            }
            catch (Exception exception)
            {
                Log.Error($"{{Url}} {{Verb}} failed: {{Exception}}", Request.Path, Request.Method, exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Gets the vessel information for quarantine.
        /// </summary>
        /// <returns></returns>
        [ResourcePermission("EmailRestriction", AccessRights.Read)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(QuarantineVesselConfig), "The operation was successful.")]
        [HttpGet, Route("email/settings/quarantine/vessels")]
        public async Task<IActionResult> GetVesselList()
        {
            try
            {
                var result = await _dbRepository.GetVesselConfigurationList(CompanyId);
                return base.Ok(result);

            }
            catch (Exception exception)
            {
                Log.Error($"{{Url}} {{Verb}} failed: {{Exception}}", Request.Path, Request.Method, exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Gets the vessel information.
        /// </summary>
        /// <returns></returns>
        [ResourcePermission("EmailRestriction", AccessRights.Read)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(QuarantineVesselConfig), "The operation was successful.")]
        [HttpGet, Route("email/settings/quarantine/vessels/{vesselId}")]
        public async Task<IActionResult> GetVesselInformation(long vesselId)
        {
            try
            {
                var result = await _dbRepository.GetVesselConfiguration(vesselId, CompanyId);
                return base.Ok(result);
            }
            catch (Exception exception)
            {
                Log.Error($"{{Url}} {{Verb}} failed: {{Exception}}", Request.Path, Request.Method, exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Updates the company quarantine configuration for the current logged in user.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [ResourcePermission("EmailRestriction", AccessRights.Write)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(QuarantineCompanyConfig), "The company quarantine configuration was successfully updated.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), "Something went wrong when updating the company quarantine configuration.")]
        [HttpPatch, Route("email/settings/quarantine")]
        public async Task<IActionResult> PatchCompanyConfig([FromBody] JObject json)
        {
            try
            {
                var qcc = await _dbRepository.PatchCompanyConfigAsync(json, CompanyId);
                return Ok(qcc);
            }

            catch (ValidationException exception)
            {
                Log.Warning($"{{Url}} {{Verb}} failed: {{result}}", Request.Path, Request.Method, exception.Message);
                return new BadRequestObjectResult(new ErrorObject { Message = exception.Message });
            }

            catch (Exception exception)
            {
                Log.Error($"{{Url}} {{Verb}} failed: {{Exception}}", Request.Path, Request.Method, exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Updates the quarantine configuration for the specified vessel.
        /// </summary>
        /// <param name="vesselId">The id of the vessel to update quarantine configuration.</param>
        /// <param name="json"></param>
        /// <returns></returns>
        [ResourcePermission("EmailRestriction", AccessRights.Write)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(QuarantineCompanyConfig), "The vessel quarantine configuration was successfully updated.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), "Something went wrong when updating the vessel quarantine configuration.")]
        [HttpPatch, Route("email/settings/quarantine/{vesselId}")]
        public async Task<IActionResult> PatchVesselConfig(long vesselId, [FromBody] JObject json)
        {
            try
            {
                var qcc = await _dbRepository.PatchVesselConfigAsync(json, CompanyId, vesselId);
                return Ok(qcc);
            }


            catch (ValidationException exception)
            {
                Log.Warning($"{{Url}} {{Verb}} failed: {{result}}", Request.Path, Request.Method, exception.Message);
                return new BadRequestObjectResult(new ErrorObject { Message = exception.Message });
            }

            catch (Exception exception)
            {
                Log.Error($"{{Url}} {{Verb}} failed: {{Exception}}", Request.Path, Request.Method, exception);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

    }
}