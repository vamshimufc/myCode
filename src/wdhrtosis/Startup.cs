using Boxed.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CorrelationId;
using wdhrtosis.Data;
using Microsoft.EntityFrameworkCore;
using wdhrtosis.Options;
using System.Reflection;

namespace wdhrtosis
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment hostingEnvironment;
        public readonly Assembly mainAssembly;

        public static IConfiguration StaticConfig { get; private set; }
        public static bool TaskWasSuccess { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this.configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
            this.mainAssembly = typeof(Startup).GetTypeInfo().Assembly;
            StaticConfig = configuration;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddCustomOptions(configuration)
                .AddDbContext<PersonImportContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("SSISImportConnectionString"));
                })
                .AddDbContextPool<WorkdayImportContext>(options =>
                {
                 options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
                });

            var authOptions = GetAuthOptionsFromConfiguration(services.BuildServiceProvider());

            if (hostingEnvironment.IsDevelopment())
            {
                services
                    .AddCustomIdentityServer(authOptions.ProtectedResourceName);
            }

            services
                .AddCustomIdentityServerAuthentication(authOptions.Authority, authOptions.ProtectedResourceName);

            services
                .AddCorrelationIdFluent()
                //                .AddCustomHealthChecks()
                .AddCustomSwagger(authOptions.Authority, authOptions.ProtectedResourceName, mainAssembly)
                .AddCustomVersioning()
                .AddMvcCore()
                    .AddApiExplorer()
                    .AddAuthorization()
                    .AddDataAnnotations()
                    .AddJsonFormatters()
                    .AddCustomJsonOptions(this.hostingEnvironment)
                    .AddCustomCors()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMvcCore().AddVersionedApiExplorer();  //https://github.com/Microsoft/aspnet-api-versioning/issues/330
            services
                .AddCustomAuthorizationAndPolicies(authOptions.ProtectedResourceName)
                .AddProjectServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder application,WorkdayImportContext dbcontext)
        {
            dbcontext.Database.EnsureCreated();

            var authOptions = GetAuthOptionsFromConfiguration(application.ApplicationServices);

            application
                // Pass a GUID in a X-Correlation-ID HTTP header to set the HttpContext.TraceIdentifier.
                // UpdateTraceIdentifier must be false due to a bug. See https://github.com/aspnet/AspNetCore/issues/5144
                .UseCorrelationId(new CorrelationIdOptions()
                {
                    UseGuidForCorrelationId = true,
                    UpdateTraceIdentifier = false
                })
                .UseIf(
                    this.hostingEnvironment.IsDevelopment(),
                    x => x.UseDeveloperErrorPages())
                //                .UseHealthChecks("/status")
                //                .UseHealthChecks("/status/self", new HealthCheckOptions() { Predicate = _ => false })
                .UseIf(
                    this.hostingEnvironment.IsDevelopment(),
                    x => x.UseIdentityServer())
                .UseAuthentication()
                .UseMvc()
                .UseSwagger(
                    options =>
                    {
                        options.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
                    })

                // New middleware from the Serilog team to be released in Serilog version 3.0.x
                // The middleware filters out all the normal Microsoft logging noise during a request and response
                // This cuts down on verbostiy quite a bit.
                // Uncomment when avalible.
                // .UseSerilogRequestLogging()
                .UseCustomSwaggerUI(hostingEnvironment, authOptions.ProtectedResourceName, mainAssembly);
        }

        private AuthOptions GetAuthOptionsFromConfiguration(System.IServiceProvider services)
        {
            return services.GetRequiredService<AuthOptions>();
        }
    }
}
