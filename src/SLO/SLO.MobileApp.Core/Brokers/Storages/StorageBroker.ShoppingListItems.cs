using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Storages;

internal partial class StorageBroker
{
    public async ValueTask<ShoppingListItem> InsertShoppingListItemAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken)
    {
        return await InsertAsync(
            item: shoppingListItem,
            cancellationToken);
    }
}
