using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Serilog;
using System.Net;

namespace FTConfig
{
    public class Program
    {

        private static string regKey = @"SOFTWARE\DUALOG\FtConfig";

        public static int GetFromReg(string name, int defaultValue = 0)
        {
            var reg = Registry.LocalMachine.OpenSubKey(regKey);
            if (null == reg)
            {
                Log.Debug("Failed to read API port number from Registry Key: " + regKey);
                return defaultValue;
            }
            var ro = (int)reg.GetValue(name, defaultValue);
            return ro;
        }


        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                options.Listen(IPAddress.Any, GetFromReg("PortNo", 98));         // http:*:101
                    //options.Listen(IPAddress.Any, 443, listenOptions =>
                    //{
                    //    listenOptions.UseHttps("certificate.pfx", "password");
                    //}
                    //);
                })
                .UseStartup<Startup>()
                .Build();

        //WebHost.CreateDefaultBuilder(args)
        //    .UseStartup<Startup>()
        //    .UseUrls("https://*:" + GetFromReg("PortNoPublic", 98))
        //    .Build();
    }
}
