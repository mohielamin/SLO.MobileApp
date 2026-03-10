using SLO.MobileApp.Brokers.Loggings;
using SLO.MobileApp.Brokers.Storages;
using SLO.MobileApp.Models.Foundations.ShoppingItems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Services.Foundations.ShoppingItems;

internal sealed partial class ShoppingItemService : IShoppingItemService
{
    private readonly IStorageBroker _storageBroker;
    private readonly ILoggingBroker _loggingBroker;

    public ShoppingItemService(
        IStorageBroker storageBroker,
        ILoggingBroker loggingBroker)
    {
        _storageBroker = storageBroker;
        _loggingBroker = loggingBroker;
    }

    public async ValueTask<ShoppingItem> AddShoppingItemAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
