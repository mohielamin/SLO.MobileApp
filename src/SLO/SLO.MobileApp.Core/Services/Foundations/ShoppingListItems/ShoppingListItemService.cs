using SLO.MobileApp.Core.Brokers.DateTimes;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;

internal partial class ShoppingListItemService : IShoppingListItemService
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
        await TryCatch(cancellationToken, async () =>
        {
            await ValidateShoppingListItemOnAddAsync(
                shoppingListItem,
                cancellationToken);

            return await _storageBroker.InsertShoppingListItemAsync(
                shoppingListItem, cancellationToken);
        });

    public async ValueTask<IQueryable<ShoppingListItem>> RetrieveAllShoppingListItemsAsync(
        CancellationToken cancellationToken) =>
        await TryCatch(cancellationToken, async () =>
        await _storageBroker.SelectAllShoppingListItemsAsync(
            cancellationToken));

    public async ValueTask<ShoppingListItem> RetrieveShoppingListItemByIdAsync(
        Guid shoppingListItemId,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
