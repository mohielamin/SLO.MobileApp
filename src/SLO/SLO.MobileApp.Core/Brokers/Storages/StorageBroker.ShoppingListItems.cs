using Microsoft.EntityFrameworkCore;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Storages;

internal partial class StorageBroker
{
    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }

    public async ValueTask<ShoppingListItem> InsertShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken) =>
        await InsertAsync(
            item: shoppingListItem,
            cancellationToken);

    public async ValueTask<IQueryable<ShoppingListItem>> SelectAllShoppingListItemsAsync(
        CancellationToken cancellationToken) =>
        await SelectAllAsync<ShoppingListItem>(cancellationToken);

    public async ValueTask<ShoppingListItem> SelectShoppingListItemByIdAsync(
        Guid shoppingListItemId,
        CancellationToken cancellationToken) =>
        await SelectByIdAsync<ShoppingListItem>(
            cancellationToken,
            ids: shoppingListItemId);

    public async ValueTask<ShoppingListItem> UpdateShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken) =>
        await UpdateAsync(
            item: shoppingListItem,
            cancellationToken);

    public async ValueTask<ShoppingListItem> DeleteShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken) =>
        await DeleteAsync(
            item: shoppingListItem,
            cancellationToken);
}
