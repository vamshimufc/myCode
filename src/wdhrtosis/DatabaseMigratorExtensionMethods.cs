using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using wdhrtosis.Data;

namespace wdhrtosis
{
    public static class DatabaseMigratorExtensionMethods
    {
        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            using (var serviceScope = webHost.Services.CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<WorkdayImportContext>())
            {
                context.Database.Migrate();
            }

            using (var serviceScope = webHost.Services.CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<PersonImportContext>())
            {
                context.Database.Migrate();
            }
            return webHost;
        }
    }
}
