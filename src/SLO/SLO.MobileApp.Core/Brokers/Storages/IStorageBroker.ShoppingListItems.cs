using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<ShoppingListItem> InsertShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken);
}
