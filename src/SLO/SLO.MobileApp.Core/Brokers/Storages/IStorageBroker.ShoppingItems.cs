using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<ShoppingItem> InsertShoppingItemAsync(
        ShoppingItem shoppingItem, CancellationToken cancellationToken);

    ValueTask<IQueryable<ShoppingItem>> SelectAllShoppingItemsAsync(
        CancellationToken cancellationToken);

    ValueTask<ShoppingItem> SelectShoppingItemByIdAsync(
        Guid shoppingItemId, CancellationToken cancellationToken);

    ValueTask<ShoppingItem> UpdateShoppingItemAsync(
        ShoppingItem shoppingItem, CancellationToken cancellationToken);

    ValueTask<ShoppingItem> DeleteShoppingItemAsync(
        ShoppingItem shoppingItem, CancellationToken cancellationToken);
}
