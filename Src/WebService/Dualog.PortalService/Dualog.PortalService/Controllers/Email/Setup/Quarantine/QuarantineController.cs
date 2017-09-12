using System;
using System.Net;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Email.Setup.Quarantine.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers.Email.Setup.Quarantine
{
    [Authorize]
    [Route("api/v1")]
    public class QuarantineController : DualogController
    {
        private QuarantineRepository _dbRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="QuarantineController"/> class.
        /// </summary>
        /// <param name="dcFactory">The dc factory.</param>
        public QuarantineController(IDataContextFactory dcFactory)
            : base( dcFactory )
        {
            _dbRepository = new QuarantineRepository(dcFactory);
        }


        /// <summary>
        /// Get quarantine settings for company.
        /// </summary>
        /// <returns></returns>
        [ResourcePermission("EmailRestriction", AccessRights.Read)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(QuarantineCompanyModel), "The operation was successful.")]
        [HttpGet, Route("email/setup/quarantine/companyquarantine")]
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
        /// Get qurantine settings for vessel.
        /// </summary>
        /// <returns></returns>
        [ResourcePermission("EmailRestriction", AccessRights.Read)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(QuarantineVesselModel), "The operation was successful.")]
        [HttpGet, Route("email/setup/quarantine/shipquarantine")]
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
        /// Get quarantine settings for one vessel.
        /// </summary>
        /// <returns></returns>
        [ResourcePermission("EmailRestriction", AccessRights.Read)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(QuarantineVesselModel), "The operation was successful.")]
        [HttpGet, Route("email/setup/quarantine/shipquarantine/{vesselId}")]
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
        /// Update the company quarantine configuration for the current logged in user.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="quarantineId"></param>
        /// <returns></returns>
        [ResourcePermission("EmailRestriction", AccessRights.Write)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(QuarantineCompanyModel), "The company quarantine configuration was successfully updated.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), "Something went wrong when updating the company quarantine configuration.")]
        [HttpPatch, Route("email/setup/quarantine/companyquarantine/{quarantineId}")]
        public Task<IActionResult> PatchCompanyConfig([FromBody] JObject json, long quarantineId)
             => this.HandlePatchAction(() => _dbRepository.PatchCompanyConfigAsync(json, CompanyId, quarantineId));


        /// <summary>
        /// Updates the quarantine configuration for the specified vessel.
        /// </summary>
        /// <param name="quarantineId">The id of the quarantine entity to update </param>
        /// <param name="json"></param>
        /// <returns></returns>
        [ResourcePermission("EmailRestriction", AccessRights.Write)]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(QuarantineCompanyModel), "The vessel quarantine configuration was successfully updated.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), "Something went wrong when updating the vessel quarantine configuration.")]
        [HttpPatch, Route("email/setup/quarantine/shipquarantine/{quarantineId}")]
        public Task<IActionResult> PatchVesselConfig([FromBody] JObject json, long quarantineId)
             => this.HandlePatchAction(() => _dbRepository.PatchVesselConfigAsync(json, CompanyId, quarantineId));

    }
}