using SLO.MobileApp.Core.Brokers.Loggings;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Processings.ShoppingListItems;

internal partial class ShoppingListItemProcessingService : IShoppingListItemProcessingService
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
        CancellationToken cancellationToken) =>
        await TryCatch(
            cancellationToken,
            async () =>
            {
                ValidateShoppingListItemOnUpsert(shoppingListItem);

                ShoppingListItem matchingShoppingListItem =
                    await RetrieveMatchingShoppingListItemAsync(
                        shoppingListItem,
                        cancellationToken);

                return matchingShoppingListItem switch
                {
                    null => await _shoppingListItemService.AddShoppingListItemAsync(
                        shoppingListItem,
                        cancellationToken),

                    { } => await _shoppingListItemService.ModifyShoppingListItemAsync(
                        shoppingListItem,
                        cancellationToken),
                };
            });

    public async ValueTask<IQueryable<ShoppingListItem>> RetrieveAllShoppingListItemsByShoppingListIdAsync(
        Guid shoppingListItemId,
        CancellationToken cancellationToken)
    {
        IQueryable<ShoppingListItem> retrievedShoppingListItems =
            await _shoppingListItemService.RetrieveAllShoppingListItemsAsync(
                cancellationToken);

        return retrievedShoppingListItems.Where(shoppingListItem =>
            shoppingListItem.ShoppingListId == shoppingListItemId);
    }

    private async ValueTask<ShoppingListItem> RetrieveMatchingShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken)
    {
        IQueryable<ShoppingListItem> retrievedShoppingListItems =
            await _shoppingListItemService.RetrieveAllShoppingListItemsAsync(
            cancellationToken);

        return retrievedShoppingListItems.FirstOrDefault(match =>
            match.Id == shoppingListItem.Id);
    }
}
