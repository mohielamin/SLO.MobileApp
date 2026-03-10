using SLO.MobileApp.Models.Foundations.ShoppingItems;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Brokers.Storages;

internal sealed partial class StorageBroker
{
    public async ValueTask<ShoppingItem> InsertShoppingItemAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken) =>
        await InsertAsync(shoppingItem, cancellationToken);
}
