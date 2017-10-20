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


/*
 *
 {
    "clients": {
        "myFirstBoat": {
            "public_key": "ssh-rsa AAAAB3NzaC1yc2EAAAABIwAAAQEAps24UjNX0i9kdH+00UZUa/GpP+R0QpJMJRYIj/rwMCtaMz28mSzC9ibIpOKuDkoQ1ekB4mKOYqG+fbPwOlS3lKi23lRN1+Z/v9itYpANf/F6bBELLGx4UHty3dLyvkloe0W+epsSGSYxne2zmf3OYb8o3zh1lcloy45yD3DTIL2iSm9bgLDtA75uzEV6oGAlQbldctBP6QxoU5ynWIBzMW3OhuSOrg4wkhPMai9zkT3TYsgqyJT5zxDkAIRtb+JszK+cYnURg3/fm3FRglKwNS5I4n8y48+r7Pe7gos3NqtbgODOAtSazGRPKQvmyQANCh6F74aUyieLv3xwBZRGnw== redhog@thalarielD",
            "mounts": {
                "myFirstFolder": {
                    "source": "//someServer/somePath",
                    "type": "cifs",
                    "options": "guest,uid=1000,iocharset=utf8"
                }           
            }
        },
        "myFirstOfficeX": {
            "public_key": "ssh-rsa AAAAB3NzaC1yc2EAAAABIwAAAQEAps24UjNX0i9kdH+00UZUa/GpP+R0QpJMJRYIj/rwMCtaMz28mSzC9ibIpOKuDkoQ1ekB4mKOYqG+fbPwOlS3lKi23lRN1+Z/v9itYpANf/F6bBELLGx4UHty3dLyvkloe0W+epsSGSYxne2zmf3OYb8o3zh1lcloy45yD3DTIL2iSm9bgLDtA75uzEV6oGAlQbldctBP6QxoU5ynWIBzMW3OhuSOrg4wkhPMai9zkT3TYsgqyJT5zxDkAIRtb+JszK+cYnURg3/fm3FRglKwNS5I4n8y48+r7Pe7gos3NqtbgODOAtSazGRPKQvmyQANCh6F74aUyieLv3xwBZRGnw== redhog@thalarielC",
            "mounts": {}
        }
    },
    "jobs": {
        "myFirstFolder": {
            "sources": [
                "myFirstBoat"
            ],
            "destinations": [
                "myFirstOfficeX"
            ]
        }
    }
}
 * 
 * 
 * */

namespace FTConfig.Controllers
{

    [Produces("application/json")]
    [Route("api/v1/FtvConfig")]
    public class FtvConfigController : Controller
    {

        //private FtvConfigController()
        //{
        //    Log.Debug("FtvConfigController CONSTRUCTOR");
        //}

        public struct ConfigArg
        {
            public long minimumClientId;
        }

        public struct Mount
        {
            public string source;
            public string type;
            public string options;
        }

        public struct Client
        {
            public string public_key;
            public Dictionary<string, Mount> mounts;
        }

        public class Folder
        {
            string client;
            string mount_type;
            string mount_options;
        }

        public struct Job
        {
            public List<string> sources;
            public List<string> destinations;
        }

        public class ConfigResponse
        {
            public Dictionary<string, Client> clients;
            public Dictionary<string, Job> jobs;
        }

        static private ConfigResponse fakedConfig_;

        [HttpPost, Route("SetConfig")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(bool), "Set a fake config for subsequent response.")]
        public IActionResult SetConfig([FromBody] ConfigResponse arg)
        {
            fakedConfig_ = arg;
            return Ok(true);
        }


        [HttpPost, Route("GetConfig")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ConfigResponse), "Get All config")]
        public IActionResult GetConfig([FromBody] ConfigArg arg)
        {
            Log.Debug("GetConfig(" + arg.minimumClientId + ")");

            if (null != fakedConfig_)
                return Ok(fakedConfig_);

            var cr = new ConfigResponse();
            cr.clients = new Dictionary<string, Client>();


            // myFirstBoat 
            var myFirstBoat = new Client() {
                public_key = "ssh-rsa AAAAB3NzaC1yc2EAAAABIwAAAQEAps24UjNX0i9kdH+00UZUa/GpP+R0QpJMJRYIj/rwMCtaMz28mSzC9ibIpOKuDkoQ1ekB4mKOYqG+fbPwOlS3lKi23lRN1+Z/v9itYpANf/F6bBELLGx4UHty3dLyvkloe0W+epsSGSYxne2zmf3OYb8o3zh1lcloy45yD3DTIL2iSm9bgLDtA75uzEV6oGAlQbldctBP6QxoU5ynWIBzMW3OhuSOrg4wkhPMai9zkT3TYsgqyJT5zxDkAIRtb+JszK+cYnURg3/fm3FRglKwNS5I4n8y48+r7Pe7gos3NqtbgODOAtSazGRPKQvmyQANCh6F74aUyieLv3xwBZRGnw== redhog@thalarielD",
                mounts = new Dictionary<string, Mount>()
            };

            var myFirstFolder = new Mount() {
                source = "//someServer/somePath",
                options = "guest,uid=1000,iocharset=utf8",
                type = "cifs" };
            myFirstBoat.mounts.Add("myFirstFolder", myFirstFolder);

            cr.clients.Add("myFirstBoat", myFirstBoat);

            // myFirstOfficeX 
            var myFirstOfficeX = new Client()
            {
                public_key = "ssh-rsa AAAAB3NzaC1yc2EAAAABIwAAAQEAps24UjNX0i9kdH+00UZUa/GpP+R0QpJMJRYIj/rwMCtaMz28mSzC9ibIpOKuDkoQ1ekB4mKOYqG+fbPwOlS3lKi23lRN1+Z/v9itYpANf/F6bBELLGx4UHty3dLyvkloe0W+epsSGSYxne2zmf3OYb8o3zh1lcloy45yD3DTIL2iSm9bgLDtA75uzEV6oGAlQbldctBP6QxoU5ynWIBzMW3OhuSOrg4wkhPMai9zkT3TYsgqyJT5zxDkAIRtb+JszK+cYnURg3/fm3FRglKwNS5I4n8y48+r7Pe7gos3NqtbgODOAtSazGRPKQvmyQANCh6F74aUyieLv3xwBZRGnw== redhog@thalarielC",
                mounts = new Dictionary<string, Mount>()
            };

            cr.clients.Add("myFirstOfficeX", myFirstBoat);


            cr.jobs = new Dictionary<string, Job>();
            var myFirstJob = new Job();
            myFirstJob.destinations = new List<string>();
            myFirstJob.destinations.Add("myFirstBoat");

            myFirstJob.sources = new List<string>();
            myFirstJob.sources.Add("myFirstOfficeX");

            cr.jobs.Add("myFirstFolder", myFirstJob);

            return Ok(cr);
        }


    }


    [Produces("application/json")]
    [Route("api/v1/Test")]
    public class TestController : Controller
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