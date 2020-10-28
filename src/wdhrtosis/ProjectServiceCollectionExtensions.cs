using EA.Mulesoft.Extensions;
using Microsoft.Extensions.DependencyInjection;
using wdhrtosis.ImportManagers;
using wdhrtosis.Validation;

namespace wdhrtosis
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods add project services.
    /// </summary>
    /// <remarks>
    /// AddSingleton - Only one instance is ever created and returned.
    /// AddScoped - A new instance is created and returned for each request/response cycle.
    /// AddTransient - A new instance is created and returned each time.
    /// </remarks>
    public static class ProjectServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
          services
                .AddMulesoft()
                .AddTransient<IMulesoftManager, MulesoftManager>()
                .AddTransient<IPersonImportDatabaseWrapper, PersonImportDatabaseWrapper>()
                .AddTransient<INameManager, NameManager>()
                .AddTransient<IEmailAddressManager, EmailAddressManager>()
                .AddTransient<IAddressManager, AddressManager>()
                .AddTransient<IContactManager, ContactManager>()
                .AddTransient<IVisaManager, VisaManager>()
                .AddTransient<IValidationManager, ValidationManager>()
                .AddTask<DRA1>();
    }
}
