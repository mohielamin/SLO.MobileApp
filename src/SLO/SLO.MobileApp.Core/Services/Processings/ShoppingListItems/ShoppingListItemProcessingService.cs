using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Processings.ShoppingListItems;

internal class ShoppingListItemProcessingService : IShoppingListItemProcessingService
{
    private readonly IShoppingListItemService _shoppingListItemService;
    private readonly ILoggingBroker _loggingBroker;

    public ShoppingListItemProcessingService(
        IShoppingListItemService shoppingListItemService,
        ILoggingBroker loggingBroker)
    {
        _shoppingListItemService = shoppingListItemService;
        _loggingBroker = loggingBroker;
    }

    public async ValueTask<ShoppingListItem> UpsertShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken)
    {
        await _shoppingListItemService.RetrieveAllShoppingListItemsAsync(
            cancellationToken);

        return await _shoppingListItemService.AddShoppingListItemAsync(
            shoppingListItem,
            cancellationToken);
    }
}
