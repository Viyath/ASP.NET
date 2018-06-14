using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace First_App
{

    public class Startup
    {
        private readonly IConfigurationRoot configuration;

        public Startup(IHostingEnvironment env)
        {
            configuration = new ConfigurationBuilder()
                                    .AddEnvironmentVariables()
                                    .AddJsonFile(env.ContentRootPath + "/config.json")
                                    .AddJsonFile(env.ContentRootPath + "/config.development.json")
                                    .Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<FeatureToggles>(x => new FeatureToggles
            {
                EnableDeveloperExceptions = configuration.GetValue<bool>("FeatureToggle:EnableDeveloperExceptions")
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
           // ILoggerFactory loggerFactory,
            FeatureToggles features)
        {
            app.UseExceptionHandler("/error.html");            

            //if (configuration.GetValue<bool>("FeatureToggle:EnableDeveloperExceptions"))
            if (features.EnableDeveloperExceptions)
            {
                app.UseDeveloperExceptionPage();
            }
            app.Use(async (contex, next) =>
                {
                    if (contex.Request.Path.Value.Contains("invalid"))
                        throw new Exception("Error!");
                    await next();

                });
            app.UseMvc(routes =>
            {
                routes.MapRoute("Default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseFileServer();
        }
    }
}
