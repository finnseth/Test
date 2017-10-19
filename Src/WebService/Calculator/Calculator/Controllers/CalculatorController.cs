using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using Serilog;

namespace Calculator.Controllers
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
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Sum), "Sum the term1 and term2 of the argument.")]
        public IActionResult Sum([FromBody] SumArg arg)
        {
            Log.Debug("Sum(" + arg.term1 + ", " + arg.term2 + ")");

            Sum s; s.v = 0;
            s.v = arg.term1 + arg.term2;
            return Ok(s);
        }
    }

    public class CacheSum
    {
        public int Sum { get; set; }
    }

    /// <summary>
    /// Jadajada.
    /// </summary>
    public struct SumArg
    {
        /// <summary>
        /// Arg represent the argument we require for the Sup operation. Contain term1 and term2 to be summed
        /// </summary>
        /// <description>
        /// Hope that you find this comment useful.
        /// When this comment is present the green blurred underline is removed from under the v declaration below.
        /// </description>
        public int term1;
        public int term2;
    }


    /// <summary>
    /// Jadajada.
    /// </summary>
    public struct Sum
    {
        /// <summary>
        /// v holds the valu of the sum. Hope that you find this comment useful. When this comment is present the green blurred underline is removed from 
        /// </summary>
        /// <description>
        /// Hope that you find this comment useful.
        /// When this comment is present the green blurred underline is removed from under the v declaration below.
        /// </description>
        public int v;
    }

}
