using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Configurations;
using System;
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
        string mobileDatabaseFilePath =
            Environment.GetEnvironmentVariable(
                variable: "MOBILE_DB_PATH");

        if (string.IsNullOrWhiteSpace(mobileDatabaseFilePath))
        {
            mobileDatabaseFilePath =
                   "../SLO.MobileApp/Resources/Raw/SloMobileAppDbV1.db";
        }

        return new LocalConfiguration
        {
            DatabaseFilePath = mobileDatabaseFilePath,
        };
    }
}
