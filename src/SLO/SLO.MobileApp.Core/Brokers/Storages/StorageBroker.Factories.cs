using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using SLO.MobileApp.Core.Models.Configurations;

namespace SLO.MobileApp.Core.Brokers.Storages
{
    internal class StorageBrokerDbContextFactory : IDesignTimeDbContextFactory<StorageBroker>
    {
        public StorageBroker CreateDbContext(string[] args)
        {
            IOptions<LocalConfiguration> localConfigurationOptions =
                Options.Create(new LocalConfiguration { DatabaseFilePath = "" });

            return new StorageBroker(localConfigurationOptions);
        }
    }
}
