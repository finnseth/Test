
using Dualog.Data.Entity;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.SwaggerGen;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dualog.PortalService.Controllers.Nippon.Testing
{

    /// <summary>
    /// Defines endpoints to work with User resources
    /// </summary>
    //[Authorize]
    [Route("api/v25/Calcualtor")]

    public class PlusController : DualogController
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainController"/> class.
        /// </summary>
        public PlusController(IDataContextFactory dcFactory)
            : base(dcFactory)
        {
        }

        /// <summary>
        /// Operation Plus(a, b); Supposed to return a + b
        /// </summary>
        [HttpPost, Route("Plus")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Sum), "Sum of all integers in the input array.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(Error), "Bad or empty request.")]
        public IActionResult AllAsync([FromBody] int[] args)
        {
            Sum res; res.v = 0;
            if (args == null)
            {
                Error e; e.what = "Bad or empty request.";
                return BadRequest(e);
            }
            foreach (int arg in args)
            {
                res.v += arg;
            }

            return Ok(res);
        }
    }

    public struct Error
    {
        public string what;
    }

    public class Plus
    {
        public int a, b;
    }


    public struct Sum
    {
        public int v;
    }


}
