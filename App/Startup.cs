using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Datasilk.Core.Extensions;

namespace Legendary
{
    public class Startup
    {
        private static IConfigurationRoot config;

        public virtual void ConfigureServices(IServiceCollection services)
        {
            //set up Server-side memory cache
            services.AddDistributedMemoryCache();
            services.AddMemoryCache();

            //configure request form options
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            //add session
            services.AddSession();

            //add health checks
            services.AddHealthChecks();

            ViewPartialPointers.Paths.Add(new System.Collections.Generic.KeyValuePair<string, string>("shared", "Views/Shared"));
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Server.IsDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";


            //get environment based on application build
            switch (env.EnvironmentName.ToLower())
            {
                case "production":
                    Server.environment = Server.Environment.production;
                    break;
                case "staging":
                    Server.environment = Server.Environment.staging;
                    break;
                default:
                    Server.environment = Server.Environment.development;
                    break;
            }

            //load application-wide cache
            var configFile = "config" +
                (Server.IsDocker ? ".docker" : "") +
                (Server.environment == Server.Environment.production ? ".prod" : "") + ".json";

            config = new ConfigurationBuilder()
                .AddJsonFile(Server.MapPath(configFile))
                .AddEnvironmentVariables().Build();

            Server.config = config;

            //configure Server defaults
            Server.hostUri = config.GetSection("hostUri").Value;
            var servicepaths = config.GetSection("servicePaths").Value;
            if (servicepaths != null && servicepaths != "")
            {
                Server.servicePaths = servicepaths.Replace(" ", "").Split(',');
            }
            if (config.GetSection("version").Value != null)
            {
                Server.Version = config.GetSection("version").Value;
            }

            //configure Server database connection strings
            Query.Sql.ConnectionString = config.GetSection("sql:" + config.GetSection("sql:Active").Value).Value;

            //configure Server security
            Server.bcrypt_workfactor = int.Parse(config.GetSection("Encryption:bcrypt_work_factor").Value);
            Server.salt = config.GetSection("Encryption:salt").Value;

            //configure cookie-based authentication
            var expires = !string.IsNullOrWhiteSpace(config.GetSection("Session:Expires").Value) ? int.Parse(config.GetSection("Session:Expires").Value) : 60;

            //use session
            var sessionOpts = new SessionOptions();
            sessionOpts.Cookie.Name = "Legendary";
            sessionOpts.IdleTimeout = TimeSpan.FromMinutes(expires);

            app.UseSession(sessionOpts);

            //handle static files
            var provider = new FileExtensionContentTypeProvider();

            // Add static file mappings
            provider.Mappings[".svg"] = "image/svg";
            var options = new StaticFileOptions
            {
                ContentTypeProvider = provider
            };
            app.UseStaticFiles(options);

            //exception handling
            if (Server.environment == Server.Environment.development)
            {
                app.UseDeveloperExceptionPage(new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 10
                });
            }
            else
            {
                //use HTTPS
                app.UseHsts();
                app.UseHttpsRedirection();

                //use health checks
                app.UseHealthChecks("/health");
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //set up database
            Server.hasAdmin = Query.Users.HasAdmin();

            //run Datasilk Core MVC Middleware
            app.UseDatasilkMvc(new MvcOptions()
            {
                InvokeNext = false,
                WriteDebugInfoToConsole = true,
                IgnoreRequestBodySize = true,
                Routes = new Routes()
            });

            Console.WriteLine("Running Legendary Server in " + Server.environment.ToString() + " environment");
        }
    }
}
