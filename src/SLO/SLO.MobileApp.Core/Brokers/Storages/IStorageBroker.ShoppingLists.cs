using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<ShoppingList> InsertShoppingListAsync(
        ShoppingList shoppingList,
        CancellationToken cancellationToken);

    ValueTask<IQueryable<ShoppingList>> SelectAllShoppingListsAsync(
        CancellationToken cancellationToken);

    ValueTask<ShoppingList> SelectShoppingListByIdAsync(
        Guid shoppingListId,
        CancellationToken cancellationToken);
}
