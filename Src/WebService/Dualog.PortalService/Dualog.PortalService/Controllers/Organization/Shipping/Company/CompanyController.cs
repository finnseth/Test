using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Organization.Shipping.Company.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Company
{
    [Authorize]
    [Route("api/v1")]
    public class CompanyController : DualogController
    {
        CompanyRepository _dbRepository;

        public CompanyController(IDataContextFactory dcFactory) : base( dcFactory )
        {
            _dbRepository = new CompanyRepository(dcFactory);
        }

        /// <summary>
        /// Gets specific company
        /// </summary>
        [HttpGet, Route("organization/shipping/company")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(CompanyModel), "The operation was successful.")]
        public async Task<IActionResult> GetMyCompany()
        {
            return Ok(await _dbRepository.GetCompany(CompanyId));
        }

        /// <summary>
        /// Get all companies
        /// </summary>
        [IsInDualog]
        [HttpGet, Route("organization/shipping/company/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(CompanyModel), "The operation was successful.")]
        public async Task<IActionResult> GetCompany(long id)
        {
            return Ok(await _dbRepository.GetCompanyDetailed(CompanyId, id));
        }


        /// <summary>
        /// Patch setting for a company with the given id.
        /// </summary>
        /// <param name="json">The patch document.</param>
        /// <returns></returns>
        [HttpPatch, Route("organization/shipping/company")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(CompanyModel), "The operation was successful.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(ErrorObject), Description = "The operation failed because of an invalid patch document.")]
        public async Task<IActionResult> PatchSetting([FromBody] JObject json)
        {
            try
            {
                var setting = await _dbRepository.PatchCompanyAsync(json, CompanyId);
                return Ok(setting);
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



    }
}
