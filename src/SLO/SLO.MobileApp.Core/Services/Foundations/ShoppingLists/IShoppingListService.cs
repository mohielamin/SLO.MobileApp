using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingLists;

public interface IShoppingListService
{
    ValueTask<ShoppingList> AddShoppingListAsync(
        ShoppingList shoppingList,
        CancellationToken cancellationToken);

    ValueTask<IQueryable<ShoppingList>> RetrieveAllShoppingListsAsync(
        CancellationToken cancellationToken);

    ValueTask<ShoppingList> RetrieveShoppingListByIdAsync(
        Guid shoppingListId,
        CancellationToken cancellationToken);
}
