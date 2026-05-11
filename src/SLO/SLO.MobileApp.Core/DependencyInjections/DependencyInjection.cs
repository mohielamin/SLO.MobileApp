using Microsoft.Extensions.DependencyInjection;
using SLO.MobileApp.Core.Brokers.DateTimes;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Configurations;
using SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Services.Foundations.ShoppingLists;
using SLO.MobileApp.Core.ViewModels.ShoppingLists;
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
                StorageBroker.CURRENT_DATABASE_FILE_NAME);
        });

        serviceCollection.AddBrokers();
        serviceCollection.AddFoundations();
        serviceCollection.AddSingleton<ShoppingListViewModel>();

        return serviceCollection;
    }

    private static IServiceCollection AddBrokers(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IStorageBroker, StorageBroker>();
        serviceCollection.AddTransient<IDateTimeBroker, DateTimeBroker>();
        serviceCollection.AddTransient<ILoggingBroker, LoggingBroker>();

        return serviceCollection;
    }

    private static IServiceCollection AddFoundations(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IShoppingListItemService,
            ShoppingListItemService>();

        serviceCollection.AddScoped<IShoppingListService,
            ShoppingListService>();

        return serviceCollection;
    }
}
