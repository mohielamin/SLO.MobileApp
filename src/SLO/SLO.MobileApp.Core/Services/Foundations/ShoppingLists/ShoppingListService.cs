using SLO.MobileApp.Core.Brokers.DateTimes;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingLists;

internal partial class ShoppingListService : IShoppingListService
{
    private readonly IStorageBroker _storageBroker;
    private readonly IDateTimeBroker _dateTimeBroker;
    private readonly ILoggingBroker _loggingBroker;

    public ShoppingListService(
        IStorageBroker storageBroker,
        IDateTimeBroker dateTimeBroker,
        ILoggingBroker loggingBroker)
    {
        _storageBroker = storageBroker;
        _dateTimeBroker = dateTimeBroker;
        _loggingBroker = loggingBroker;
    }

    public async ValueTask<ShoppingList> AddShoppingListAsync(
        ShoppingList shoppingList,
        CancellationToken cancellationToken) =>
        await TryCatch(cancellationToken, async () =>
        {
            await ValidateShoppingListOnAddAsync(
                shoppingList,
                cancellationToken);

            return await _storageBroker.InsertShoppingListAsync(
                shoppingList,
                cancellationToken);
        });

    public async ValueTask<IQueryable<ShoppingList>> RetrieveAllShoppingListsAsync(
        CancellationToken cancellationToken) =>
        await TryCatch(cancellationToken, async () =>
        await _storageBroker.SelectAllShoppingListsAsync(
                cancellationToken));

    public async ValueTask<ShoppingList> RetrieveShoppingListByIdAsync(
        Guid shoppingListId,
        CancellationToken cancellationToken) =>
        await TryCatch(cancellationToken, async () =>
        {
            ValidateShoppingListOnRetrieveById(shoppingListId);

            return await _storageBroker.SelectShoppingListByIdAsync(
                shoppingListId,
                cancellationToken);
        });
}
