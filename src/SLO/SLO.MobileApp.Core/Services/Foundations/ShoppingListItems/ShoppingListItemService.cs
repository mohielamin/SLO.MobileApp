using SLO.MobileApp.Core.Brokers.DateTimes;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;

internal class ShoppingListItemService : IShoppingListItemService
{
    private readonly IStorageBroker _storageBroker;
    private readonly IDateTimeBroker _dateTimeBroker;
    private readonly ILoggingBroker _loggingBroker;

    public ShoppingListItemService(
        IStorageBroker storageBroker,
        IDateTimeBroker dateTimeBroker,
        ILoggingBroker loggingBroker)
    {
        _storageBroker = storageBroker;
        _dateTimeBroker = dateTimeBroker;
        _loggingBroker = loggingBroker;
    }

    public async ValueTask<ShoppingListItem> AddShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken) =>
        await _storageBroker.InsertShoppingListItemAsync(
            shoppingListItem, cancellationToken);
}
