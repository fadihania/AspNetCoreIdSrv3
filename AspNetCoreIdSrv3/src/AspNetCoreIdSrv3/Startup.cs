using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using IdentityServer3.Core.Configuration;
using AspNetCoreIdSrv3.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens.Jwt;

namespace AspNetCoreIdSrv3
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.Map("/auth", authApp =>
            {
                var certFile = env.ApplicationBasePath + "\\idsrv3test.pfx";
                var idsrvOptions = new IdentityServerOptions
                {
                    Factory = new IdentityServerServiceFactory()
                                    .UseInMemoryUsers(Users.Get())
                                    .UseInMemoryClients(Clients.Get())
                                    .UseInMemoryScopes(Scopes.Get()),
                    SigningCertificate = new X509Certificate2(certFile, "idsrv3test"),
                    RequireSsl = false
                };
                authApp.UseIdentityServer(idsrvOptions);
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

            app.UseJwtBearerAuthentication(options =>
            {
                options.Authority = "http://localhost:5000/auth";
                options.RequireHttpsMetadata = false;

                options.Audience = "http://localhost:5000/auth/resources";
                options.AutomaticAuthenticate = true;
            });

            app.UseStaticFiles();

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
