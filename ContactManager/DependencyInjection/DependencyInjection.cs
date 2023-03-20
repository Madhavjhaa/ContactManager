using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Services;
using Npgsql;

namespace ContactManager.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterAllDependencies(this IServiceCollection serviceCollection, IConfiguration config)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            serviceCollection.AddScoped<IContactService, ContactService>();
            serviceCollection.AddTransient(x => new NpgsqlConnection(config.GetConnectionString("PostgreSql")));

            return serviceCollection;
        }
    }
}