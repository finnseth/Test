using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using Serilog;
using System.Data.SqlClient;

namespace FTConfig.Controllers
{

    [Produces("application/json")]
    [Route("api/v1/FtvConfig")]
    public class FtvConfigController : Controller
    {
        public struct PingArg
        {
            public string greetings;
        }
        struct PingResponse
        {
            public string response;
        }

        [HttpPost, Route("Ping")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(PingResponse), "Ping to check the connection.")]
        public IActionResult Ping([FromBody] PingArg arg)
        {
            Log.Debug("Ping");
            PingResponse pr;
            pr.response = arg.greetings + " to you too."; ;
            return Ok(pr);
        }


    }


}