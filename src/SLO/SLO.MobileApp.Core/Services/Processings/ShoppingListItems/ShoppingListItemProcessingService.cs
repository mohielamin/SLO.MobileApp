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
        Guid shoppingListId,
        CancellationToken cancellationToken) =>
        await TryCatch(
            cancellationToken,
            async () =>
            {
                ValidateShoppingListItemOnRetrieveAllByShoppingListId(
                    shoppingListId);

                IQueryable<ShoppingListItem> retrievedShoppingListItems =
                    await _shoppingListItemService.RetrieveAllShoppingListItemsAsync(
                        cancellationToken);

                return retrievedShoppingListItems.Where(shoppingListItem =>
                    shoppingListItem.ShoppingListId == shoppingListId);
            });

    public async ValueTask<ShoppingListItem> RemoveShoppingListItemByIdAsync(
        Guid shoppingListItemId,
        CancellationToken cancellationToken) =>
        await TryCatch(
            cancellationToken,
            async () =>
            {
                ValidateShoppingListItemOnRemoveById(shoppingListItemId);

                return await _shoppingListItemService.RemoveShoppingListItemByIdAsync(
                    shoppingListItemId,
                    cancellationToken);
            });

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
