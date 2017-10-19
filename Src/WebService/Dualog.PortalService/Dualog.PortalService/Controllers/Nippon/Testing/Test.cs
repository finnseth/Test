using Dualog.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dualog.PortalService.Controllers.Nippon
{

    /// <summary>
    /// Defines endpoints to work with User resources
    /// </summary>
    //[Authorize]
    [Route("api/v1/Testing")]

    public class TestController : DualogController
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainController"/> class.
        /// </summary>
        public TestController(IDataContextFactory dcFactory)
            : base(dcFactory)
        {
        }


        /// <summary>
        /// This is a test api for Asgeir
        /// </summary>
        [HttpGet, Route("test")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(String), "The operation was successful.")]
        public IActionResult AllAsync()
        {
            return Ok("testing testing sdfjkhsh");
        }

    }



    public class Test
    {
    }
}
