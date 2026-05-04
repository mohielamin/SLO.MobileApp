using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;

public interface IShoppingListItemService
{
    ValueTask<ShoppingListItem> AddShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken);

    ValueTask<IQueryable<ShoppingListItem>> RetrieveAllShoppingListItemsAsync(
        CancellationToken cancellationToken);
}
