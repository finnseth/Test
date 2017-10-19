using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Serilog;

using Dapper;


using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

using Dualog.DB;

namespace CacheCalculator.Controllers
{
 

    [Produces("application/json")]
    [Route("api/v2/CacheCalculator")]
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
            Log.Debug("Sum");

            Sum s; s.v = 0;
            SqlConnection db = null;

            var cr = new Dualog.DB.ConnectRecord("localhost", 0, "db1", "sa", "sa12");
            var cs = cr.GetConnectString("SQLEXPRESS");
            if (!System.String.IsNullOrEmpty(cs))
            {
                db = new SqlConnection(cs);

                if (null != db)
                {
                    var res = db.Query<CacheSum>(
                        @"select top 1 sum from Cache" +
                        " where" +
                        " (" +
                        "    (term1 = @arg1) and (term2 = @arg2)" +
                        " ) or (" +
                        "    (term1 = @arg2) and (term2 = @arg1)" +
                        " )",
                        new { arg1 = arg.term1, arg2 = arg.term2 }
                        );

                    if (res.Any())
                    {
                        var f = res.First();
                        s.v = f.Sum;
                        return Ok(s);
                    }
                }
            }

            s.v = RemoteSum(arg);
            if (null != db)
            {
                db.Execute(@"insert into Cache(term1, term2, sum) values(@term1, @term2, @s)",
                    new { term1 = arg.term1, term2 = arg.term2, s = s.v });
            }

            return Ok(s);
        }


        private int RemoteSum(SumArg arg)
        {
            var apiInstance = new CalculatorApi("http://localhost:89");
            var remoteArg = new IO.Swagger.Model.SumArg(arg.term1, arg.term2); // SumArg |  (optional) 
            try
            {
                // Sum takes an array of integer arguments and return their sum.
                var result = apiInstance.ApiV2CalculatorSumPost(remoteArg);
                return(int) result.V;
            }
            catch (Exception e)
            {
                Log.Error("Exception when calling CalculatorApi.ApiV2CalculatorSumPost: " + e.Message);
            }

            return 0;
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
