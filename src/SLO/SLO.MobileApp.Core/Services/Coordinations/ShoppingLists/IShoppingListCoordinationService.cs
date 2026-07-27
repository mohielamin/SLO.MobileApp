using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Coordinations.ShoppingLists;

public interface IShoppingListCoordinationService
{
    ValueTask<IQueryable<ShoppingListItem>> RetrieveAllShoppingListItemsByIdAsync(
        Guid shoppingListId,
        CancellationToken cancellationToken);
}
