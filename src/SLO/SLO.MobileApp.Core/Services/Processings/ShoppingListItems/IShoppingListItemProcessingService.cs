using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Processings.ShoppingListItems;

public interface IShoppingListItemProcessingService
{
    ValueTask<ShoppingListItem> UpsertShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken);

    ValueTask<IQueryable<ShoppingListItem>> RetrieveAllShoppingListItemsByShoppingListIdAsync(
        Guid shoppingListId,
        CancellationToken cancellationToken);
}
