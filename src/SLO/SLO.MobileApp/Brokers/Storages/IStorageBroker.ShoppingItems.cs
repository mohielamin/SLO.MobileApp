using SLO.MobileApp.Models.Foundations.ShoppingItems;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<ShoppingItem> InsertShoppingItemAsync(
        ShoppingItem shoppingItem, CancellationToken cancellationToken);
}
