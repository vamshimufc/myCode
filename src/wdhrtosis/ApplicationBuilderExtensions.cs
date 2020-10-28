using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace wdhrtosis
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds developer friendly error pages for the application which contain extra debug and exception information.
        /// Note: It is unsafe to use this in production.
        /// </summary>
        public static IApplicationBuilder UseDeveloperErrorPages(this IApplicationBuilder application) =>
            application
                // When a database error occurs, displays a detailed error page with full diagnostic information. It is
                // unsafe to use this in production. Uncomment this if using a database.
                // .UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
                // When an error occurs, displays a detailed error page with full diagnostic information.
                // See http://docs.asp.net/en/latest/fundamentals/diagnostics.html
                .UseDeveloperExceptionPage();

        public static IApplicationBuilder UseCustomSwaggerUI(this IApplicationBuilder application,
            IHostingEnvironment environment,
            string protectedResourceName,
            Assembly assembly) =>
            application.UseSwaggerUI(
                options =>
                {
                    var assemblyProduct = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

                    // Set the Swagger UI browser document title.
                    options.DocumentTitle = typeof(Startup)
                        .Assembly
                        .GetCustomAttribute<AssemblyProductAttribute>()
                        .Product;
                    // Set the Swagger UI to render at '/swagger' The Welcome page renders at /.
                    options.RoutePrefix = "swagger"; // string.Empty;
                    // Show the request duration in Swagger UI.
                    options.DisplayRequestDuration();

                    var provider = application.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
                    foreach (var apiVersionDescription in provider
                        .ApiVersionDescriptions
                        .OrderByDescending(x => x.ApiVersion))
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                            $"Version {apiVersionDescription.ApiVersion}");
                    }

                    if (environment.IsDevelopment())
                    {
                        var client = DevelopmentIdentityServerConfiguration.GetClients(protectedResourceName).First();
                        options.OAuthClientId(client.ClientId);
                        options.OAuthClientSecret("secret");
                    }
                    options.OAuthAppName($"{assemblyProduct} - Swagger");
                });
    }
}
