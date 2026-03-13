using SLO.MobileApp.Core.Brokers.DateTimes;
using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Brokers.Storages;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingItems;

internal sealed partial class ShoppingItemService : IShoppingItemService
{
    private readonly IStorageBroker _storageBroker;
    private readonly IDateTimeBroker _dateTimeBroker;
    private readonly ILoggingBroker _loggingBroker;

    public ShoppingItemService(
        IStorageBroker storageBroker,
        IDateTimeBroker dateTimeBroker,
        ILoggingBroker loggingBroker)
    {
        _storageBroker = storageBroker;
        _dateTimeBroker = dateTimeBroker;
        _loggingBroker = loggingBroker;
    }

    public async ValueTask<ShoppingItem> AddShoppingItemAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken) =>
        await TryCatch(async () =>
        {
            await ValidateShoppingItemOnAddAsync(
                shoppingItem, cancellationToken);

            return await _storageBroker.InsertShoppingItemAsync(
                shoppingItem, cancellationToken);
        });

    public async ValueTask<IQueryable<ShoppingItem>> RetrieveAllShoppingItemsAsync(
        CancellationToken cancellationToken) =>
        await TryCatch(async () =>
        await _storageBroker.SelectAllShoppingItemsAsync(
            cancellationToken));

    public async ValueTask<ShoppingItem> RetrieveShoppingItemByIdAsync(
        Guid shoppingItemId,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public async ValueTask<ShoppingItem> ModifyShoppingItemAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken) =>
        await TryCatch(async () =>
        {
            await ValidateShoppingItemOnModifyAsync(
                shoppingItem, cancellationToken);

            ShoppingItem storageShoppingItem =
                await _storageBroker.SelectShoppingItemByIdAsync(
                    shoppingItemId: shoppingItem.Id,
                    cancellationToken);

            ValidateStorageShoppingItem(
                storageShoppingItem,
                shoppingItemId: shoppingItem.Id);

            ValidateAgainstStorageShoppingItem(
                shoppingItem, storageShoppingItem);

            return await _storageBroker.UpdateShoppingItemAsync(
                shoppingItem, cancellationToken);
        });
}
