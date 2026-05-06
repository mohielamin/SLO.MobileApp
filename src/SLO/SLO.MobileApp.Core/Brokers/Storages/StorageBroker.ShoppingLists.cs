using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Storages;

internal partial class StorageBroker
{
    public async ValueTask<ShoppingList> InsertShoppingListAsync(
        ShoppingList shoppingList,
        CancellationToken cancellationToken) =>
        await InsertAsync(
            item: shoppingList,
            cancellationToken);
}
