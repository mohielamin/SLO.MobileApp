using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Configurations;
using System.IO;

namespace SLO.MobileApp.Infra.DbBuilder;

internal class StorageBrokerDbFactory : IDesignTimeDbContextFactory<StorageBroker>
{
    public StorageBroker CreateDbContext(string[] args)
    {
        var localConfiguration = GetLocalConfiguration();

        if (File.Exists(path: localConfiguration.DatabaseFilePath))
        {
            File.Delete(path: localConfiguration.DatabaseFilePath);
        }

        IOptions<LocalConfiguration> locationConfigurationOptions =
            Options.Create(options: localConfiguration);

        return new StorageBroker(locationConfigurationOptions);
    }

    private static LocalConfiguration GetLocalConfiguration()
    {
        string databaseFilePath =
            $"../SLO.MobileApp/Resources/Raw/SloMobileAppDb.db" +
            $"{StorageBroker.DATABASE_DEFAULT_NAME}";

        return new LocalConfiguration
        {
            DatabaseFilePath = databaseFilePath,
        };
    }
}
