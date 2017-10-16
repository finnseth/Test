using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.Swagger;

using System.IO;
using System.Reflection;
using Serilog;

namespace CacheCalculator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            Log.Debug("Startup");

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new Info
                {
                    Title = "Caching Calculator API",
                    Version = "v2",
                    Description = "The Caching Calculator combines calculations with database caching."
                });

                options.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"));
                options.DescribeAllEnumsAsStrings();
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Dualog.Net Api v2");
            });

            app.UseMvc();
        }
    }
}
