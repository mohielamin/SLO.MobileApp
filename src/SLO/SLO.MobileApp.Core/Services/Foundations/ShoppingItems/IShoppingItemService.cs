using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingItems;

internal interface IShoppingItemService
{
    ValueTask<ShoppingItem> AddShoppingItemAsync(
        ShoppingItem shoppingItem, CancellationToken cancellationToken);

    ValueTask<IQueryable<ShoppingItem>> RetrieveAllShoppingItemsAsync(
        CancellationToken cancellationToken);

    ValueTask<ShoppingItem> RetrieveShoppingItemByIdAsync(
        Guid shoppingItemId, CancellationToken cancellationToken);

    ValueTask<ShoppingItem> ModifyShoppingItemAsync(
        ShoppingItem shoppingItem, CancellationToken cancellationToken);

    ValueTask<ShoppingItem> RemoveShoppingItemByIdAsync(
        Guid shoppingItemId, CancellationToken cancellationToken);
}
