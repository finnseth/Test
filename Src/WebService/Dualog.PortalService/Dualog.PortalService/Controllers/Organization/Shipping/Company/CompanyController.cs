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
    [Route("api/v1/organization/shipping/company")]
    public class CompanyController : DualogController
    {
        CompanyRepository _companyRepository;

        public CompanyController(IDataContextFactory dcFactory) : base( dcFactory )
        {
            _companyRepository = new CompanyRepository(dcFactory);
        }

        /// <summary>
        /// Returns companies user has access to
        /// </summary>
        /// <response code="200">Ok</response>
        [HttpGet, Route("")]
        //[SwaggerResponse( HttpStatusCode.OK, Type = typeof( Company ) )]
        public Task<IActionResult> All() =>
            this.HandleGetAction(() =>
                _companyRepository.GetCompany(CompanyId, HttpContext.Search()));



        /// <summary>
        /// Get all companies
        /// </summary>
        [HttpGet, Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(CompanyModel), "The operation was successful.")]

        public Task<IActionResult> GetCompany(long id) =>
            this.HandleGetAction(() =>
                _companyRepository.GetCompanyDetailed(CompanyId, id));



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
                var setting = await _companyRepository.PatchCompanyAsync(json, CompanyId);
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
