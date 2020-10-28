using System.Collections.Generic;
using CorrelationId;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Boxed.AspNetCore.Swagger;
using Boxed.AspNetCore.Swagger.OperationFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System.Reflection;
using wdhrtosis.OperationFilter;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Authorization;
using wdhrtosis.Options;
using Microsoft.Extensions.Configuration;
using Boxed.AspNetCore;

namespace wdhrtosis
{
    public static class CustomServiceCollectionExtensions
    {

        public static IServiceCollection AddCorrelationIdFluent(this IServiceCollection services)
        {
            services.AddCorrelationId();
            return services;
        }

//        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services) =>
//            services
                //.AddHealthChecks()
//                // Add health checks for external dependencies here. See https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
//                .Services;

        public static IServiceCollection AddCustomIdentityServerAuthentication(this IServiceCollection services,
            string authority,
            string clientId)
        {
            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = false;

                    options.ApiName = clientId;
                });

            return services;
        }

        public static IServiceCollection AddCustomVersioning(this IServiceCollection services) =>
            services.AddApiVersioning(
                options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                });

        /// <summary>
        /// Adds Swagger services and configures the Swagger services.
        /// </summary>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services,
            string authority,
            string protectedResourceName,
            Assembly assembly) =>
            services.AddSwaggerGen(
                options =>
                {
                    // In Swagger, you can describe how your API is secured by defining one or more security schemes (e.g basic, api key, oauth2 etc.)
                    // and declaring which of those schemes are applicable globally OR for specific operations.
                    //https://github.com/domaindrivendev/Swashbuckle.AspNetCore#add-security-definitions-and-requirements
                    options.AddSecurityDefinition(IdentityServerAuthenticationDefaults.AuthenticationScheme, new OAuth2Scheme
                    {
                        Type = "oauth2",
                        Flow = "application",
                        TokenUrl = $"{authority}/connect/token",
                        Scopes = new Dictionary<string, string> {
                            {
                                $"{protectedResourceName}.full_access", "Access to start tasks."
                            },
                            {
                                $"{protectedResourceName}.read_only", "Access to status and logging."
                            },
                        }
                    });

                    // In addition to defining a scheme, you also need to indicate which operations that scheme is applicable to.
                    // You can apply schemes globally (i.e. to ALL operations) through the AddSecurityRequirement method.
                    // Requirement is <scheme, scopes[]> essentially.
                    options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                    {
                        {
                            IdentityModel.OidcConstants.AuthenticationSchemes.AuthorizationHeaderBearer,
                            new[]
                            {
                                $"{protectedResourceName}.full_access",
                                $"{protectedResourceName}.read_only",
                            }
                        }
                    });

                    var assemblyProduct = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
                    var assemblyDescription = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

                    options.DescribeAllEnumsAsStrings();
                    options.DescribeAllParametersInCamelCase();
                    options.DescribeStringEnumsInCamelCase();
                    options.EnableAnnotations();

                    // Add the XML comment file for this assembly, so its contents can be displayed.
                    options.IncludeXmlCommentsIfExists(assembly);

                    options.OperationFilter<CorrelationIdOperationFilter>();
                    options.OperationFilter<ForbiddenResponseOperationFilter>();
                    options.OperationFilter<UnauthorizedResponseOperationFilter>();

                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var apiVersionDescription in provider.ApiVersionDescriptions)
                    {
                        var info = new Info()
                        {
                            Title = assemblyProduct,
                            Description = apiVersionDescription.IsDeprecated ?
                                $"{assemblyDescription} This API version has been deprecated." :
                                assemblyDescription,
                            Version = apiVersionDescription.ApiVersion.ToString()
                        };
                        options.SwaggerDoc(apiVersionDescription.GroupName, info);
                    }
                });

        public static IServiceCollection AddCustomAuthorizationAndPolicies(this IServiceCollection services, string protectedResourceName)
        {
            return services.AddAuthorization(options =>
            {
                options.AddPolicy("read_only", builder =>
                {
                    builder
                        .RequireAuthenticatedUser()
                        // read_only OR. full_acess
                        .RequireScope($"{protectedResourceName}.read_only", $"{protectedResourceName}.full_access");
                });
                options.AddPolicy("full_access", builder =>
                {
                    builder
                        .RequireAuthenticatedUser()
                        .RequireScope($"{protectedResourceName}.full_access");
                });
            });
        }

        /// <summary>
        /// Configures the settings by binding the contents of the appsettings.json file to the specified Plain Old CLR
        /// Objects (POCO) and adding <see cref="IOptions{T}"/> objects to the services collection.
        /// </summary>
        public static IServiceCollection AddCustomOptions(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                // ConfigureSingleton registers IOptions<T> and also T as a singleton to the services collection.
                //Boxed.AspNetCore version 2.2.2 (for.NET Core 2.1) has  .ConfigureSingleton but not .ConfigureAndValidateSingleton
                .ConfigureSingleton<AuthOptions>(configuration.GetSection(nameof(ApplicationOptions.AuthOptions)))
                .ConfigureSingleton<ApplicationOptions>(configuration)
                .ConfigureSingleton<DRATaskOptions>(configuration.GetSection(nameof(ApplicationOptions.DRATaskOptions)));

 
        public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services, string protectedResourceName)
        {
            services
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(DevelopmentIdentityServerConfiguration.GetIdentityResources())
                .AddInMemoryApiResources(DevelopmentIdentityServerConfiguration.GetApis(protectedResourceName))
                .AddInMemoryClients(DevelopmentIdentityServerConfiguration.GetClients(protectedResourceName));

            return services;
        }
    }
}
