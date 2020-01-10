// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using CRM.Identity.Data;
using CRM.Identity.Models;
using CRM.Shared.CorrelationId;
using CRM.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System;
using System.Reflection;
using CRM.OpenTelemetry;

namespace CRM.Identity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        private readonly string _connString;

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
            _connString = configuration.GetConnectionString("identity");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationId();
            services.AddCustomOpenTelemetry();
            services.AddAppMetrics();
            services.AddControllersWithViews();
            services.AddHealthChecks()
                .AddIdentityServer(new Uri("http://localhost:5101"), name: "idsvr-check", tags: new string[] { "idsvr" });

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(_connString);
                options.EnableDetailedErrors(true);
                options.EnableSensitiveDataLogging(true);

            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddConfigurationStore(o => o.ConfigureDbContext = builder => builder.UseNpgsql(_connString, db => db.MigrationsAssembly(migrationsAssembly)))
                .AddOperationalStore(o =>
                {
                    o.ConfigureDbContext = builder => builder.UseNpgsql(_connString, db => db.MigrationsAssembly(migrationsAssembly));
                    o.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<ApplicationUser>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            services.AddAuthentication()
                .AddCookie()
                .AddLocalApi(o => o.ExpectedScope = "contact");
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UsePathBase(Configuration["PathBase"]);
            app.UseCorrelationId();
            app.UseAppMetrics();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
