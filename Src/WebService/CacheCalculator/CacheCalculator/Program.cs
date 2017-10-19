
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Win32;

using Serilog;
namespace CacheCalculator
{
    public class Program
    {
        private static string regKey = @"SOFTWARE\DUALOG\CacheCalculator";

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
                .UseUrls("http://*:" + GetFromReg("PortNo", 88))
                .UseStartup<Startup>()
                .Build();

        private int portNo_;
    }
}
