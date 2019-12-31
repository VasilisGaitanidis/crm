using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.GraphQL.Errors;
using CRM.Shared.Interceptors;
using HotChocolate;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTracing.Contrib.Grpc.Interceptors;

namespace CRM.Personal.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer((options) =>
                {
                    options.Authority = "http://localhost:8080/auth/realms/master";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "account";
                });
            return services;
        }

        public static IServiceCollection AddGrpc(this IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionInterceptor>();
                options.Interceptors.Add<ServerTracingInterceptor>();
                options.EnableDetailedErrors = true;
            });

            return services;
        }

        public static IServiceCollection AddGraphQL(this IServiceCollection services)
        {
            services.AddGraphQL(sp => Schema.Create(c =>
            {
                c.RegisterServiceProvider(sp);
                // c.RegisterQueryType<QueryType>();
                // c.RegisterMutationType<MutationType>();
                // c.RegisterType<AddressType>();
                // c.RegisterType<PersonInfoType>();
            }), new QueryExecutionOptions
            {
                IncludeExceptionDetails = true,
                TracingPreference = TracingPreference.Always
            })
            .AddErrorFilter<ValidationErrorFilter>();
            return services;
        }
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("personal");
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<PersonalContext>(options =>
                {
                    options.UseNpgsql(connString, npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
                    });
                });

            return services;
        }
    }
}
