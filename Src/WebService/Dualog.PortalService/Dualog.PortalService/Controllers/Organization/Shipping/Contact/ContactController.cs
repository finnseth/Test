using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Dualog.Data.Entity;
using Dualog.PortalService.Controllers.Organization.Shipping.Contact.Model;
using Dualog.PortalService.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Contact
{
    [Authorize]
    [Route("api/v1")]
    public class ManagementController : DualogController
    {
        ContactRepository _dbRepository;

        public ManagementController(IDataContextFactory dcFactory) : base(dcFactory)
        {
            _dbRepository = new ContactRepository(dcFactory);
        }


        /// <summary>
        /// Gets contact addresses for a given company for which the authorized user belongs to.
        /// </summary>
        [ResourcePermission("OrganizationContactCompanyAddressBook", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/contact/companyaddress")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(CompanyAddressModel), "The operation was successful.")]
        public async Task<IActionResult> GetCompanyAddress()
        {
            return Ok(await _dbRepository.GetCompanyAddress(CompanyId));
        }

        /// <summary>
        /// Gets contact addresses for a given ship.
        /// </summary>
        [ResourcePermission("OrganizationContactShipAddressBook", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/contact/shipaddress/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ShipAddressModel), "The operation was successful.")]
        public async Task<IActionResult> GetShipAddress(long id)
        {
            return Ok(await _dbRepository.GetShipAddress(CompanyId, id));
        }

        /// <summary>
        /// Gets contact addresses for a given user.
        /// </summary>
        [HttpGet, Route("organization/shipping/contact/useraddress/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserAddressModel), "The operation was successful.")]
        public async Task<IActionResult> GetUserAddress(long id)
        {
            return Ok(await _dbRepository.GetUserAddress(CompanyId, id));
        }

        /// <summary>
        /// Gets contact address groups for a given company for which the authorized user belongs to.
        /// </summary>
        [ResourcePermission("OrganizationContactCompanyAddressBook", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/contact/companyaddressgroup")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(CompanyAddressGroupModel), "The operation was successful.")]
        public async Task<IActionResult> GetCompanyAddressGroup()
        {
            return Ok(await _dbRepository.GetCompanyAddressGroup(CompanyId));
        }

        /// <summary>
        /// Gets contact address groups for a given ship.
        /// </summary>
        [ResourcePermission("OrganizationContactShipAddressBook", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/contact/shipaddressgroup/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ShipAddressGroupModel), "The operation was successful.")]
        public async Task<IActionResult> GetShipAddressGroup(long id)
        {
            return Ok(await _dbRepository.GetShipAddressGroup(CompanyId, id));
        }

        /// <summary>
        /// Gets contact address groups for a given user.
        /// </summary>
        [HttpGet, Route("organization/shipping/contact/useraddressgroup/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserAddressGroupModel), "The operation was successful.")]
        public async Task<IActionResult> GetUserAddressGroup(long id)
        {
            return Ok(await _dbRepository.GetUserAddressGroup(CompanyId, id));
        }

        /// <summary>
        /// Gets the setup for import address for all companies, requires admin priviliges.
        /// </summary>
        [IsInDualog]
        [ResourcePermission("OrganizationImportContact", AccessRights.Read)]
        [HttpGet, Route("organization/shipping/contact/importaddress")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ImportAddressModel), "The operation was successful.")]
        public async Task<IActionResult> GetImportAddress()
        {
            return Ok(await _dbRepository.GetImportAddress(CompanyId));
        }


    }
}
