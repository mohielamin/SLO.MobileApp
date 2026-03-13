using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Storages;

internal sealed partial class StorageBroker
{
    public async ValueTask<ShoppingItem> InsertShoppingItemAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken) =>
        await InsertAsync(item: shoppingItem, cancellationToken);

    public async ValueTask<IQueryable<ShoppingItem>> SelectAllShoppingItemsAsync(
        CancellationToken cancellationToken) =>
        await SelectAllAsync<ShoppingItem>(cancellationToken);

    public async ValueTask<ShoppingItem> SelectShoppingItemByIdAsync(
        Guid shoppingItemId, CancellationToken cancellationToken) =>
        await SelectByIdAsync<ShoppingItem>(
            cancellationToken,
            ids: shoppingItemId);

    public async ValueTask<ShoppingItem> UpdateShoppingItemAsync(
        ShoppingItem shoppingItem, CancellationToken cancellationToken) =>
        await UpdateAsync(item: shoppingItem, cancellationToken);

    public async ValueTask<ShoppingItem> DeleteShoppingItemAsync(
        ShoppingItem shoppingItem, CancellationToken cancellationToken) =>
        await DeleteAsync(item: shoppingItem, cancellationToken);
}
