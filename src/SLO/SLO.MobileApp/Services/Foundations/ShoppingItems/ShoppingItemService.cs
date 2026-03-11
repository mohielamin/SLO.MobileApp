using SLO.MobileApp.Brokers.DateTimes;
using SLO.MobileApp.Brokers.Loggings;
using SLO.MobileApp.Brokers.Storages;
using SLO.MobileApp.Models.Foundations.ShoppingItems;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Services.Foundations.ShoppingItems;

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

    public async ValueTask<ShoppingItem> ModifyShoppingItemAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken)
    {
        ShoppingItem storageShoppingItem =
            await _storageBroker.SelectShoppingItemByIdAsync(
                shoppingItemId: shoppingItem.Id,
                cancellationToken);

        return await _storageBroker.UpdateShoppingItemAsync(
            shoppingItem, cancellationToken);
    }
}
