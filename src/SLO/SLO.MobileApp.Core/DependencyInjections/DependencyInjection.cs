using Microsoft.Extensions.DependencyInjection;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Configurations;
using System.IO;

namespace SLO.MobileApp.Core.DependencyInjections;

internal static partial class DependencyInjection
{
    internal static IServiceCollection UseSloMobileAppCore(
        this IServiceCollection serviceCollection,
        string appDataDirectory)
    {
        serviceCollection.AddDbContext<StorageBroker>();

        serviceCollection.Configure<LocalConfiguration>(config =>
        {
            config.DatabaseFilePath = Path.Combine(
                appDataDirectory,
                "local.db");
        });

        serviceCollection.AddTransient<IStorageBroker, StorageBroker>();

        return serviceCollection;
    }
}
