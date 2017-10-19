using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nippon.Server.Api.Controllers.Calculator
{

    [Route("Calc")]
    public class CalculatorController : Controller
    {
        [HttpPost, Route("Sum")]
        public IActionResult Sum([FromBody] int[] args)
        {
            Sum s; s.v = 0;



            return Ok(s);
        }

    }


    public struct Sum
    {
        public int v;
    }
}
