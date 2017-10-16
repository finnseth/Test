using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using System.Net;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.SwaggerGen;
using Serilog;

namespace CacheCalculator.Controllers
{
    [Produces("application/json")]
    [Route("api/v2/Calculator")]
    public class CalculatorController : Controller
    {
        /// <summary>
        /// Sum takes an array of integer arguments and return their sum.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost, Route("Sum")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Sum), "Sum of all integers in the input array.")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(Error), "Bad or empty request.")]
        public IActionResult Sum([FromBody] int[] args)
        {
            Log.Debug("hepp");
            if (null == args)
                return BadRequest(new Error("No array argument received by server."));
            Sum s; s.v = 0;
            foreach (var a in args)
                s.v += a;

            return Ok(s);
        }

    }

    /// <summary>
    /// Jadajada.
    /// </summary>
    public struct Sum
    {
        public int v;
    }

    /// <summary>
    /// Error description holder.
    /// </summary>
    public class Error
    {
        string what_;
        public Error(string what)
        {
            what_ = what;
        }

        public string what => what_;
    }

}
