using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using System.Net;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.SwaggerGen;


namespace CacheCalculator.Controllers
{
    [Produces("application/json")]
    [Route("api/Calculator")]
    public class CalculatorController : Controller
    {
        [HttpPost, Route("Sum")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Sum), "Sum of all integers in the input array.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(Error), "Bad or empty request.")]
        public IActionResult Sum([FromBody] int[] args)
        {
            Sum s; s.v = 42;
            return Ok(s);
        }

    }

    public struct Sum
    {
        public int v;
    }

    public struct Error
    {
    }

}
