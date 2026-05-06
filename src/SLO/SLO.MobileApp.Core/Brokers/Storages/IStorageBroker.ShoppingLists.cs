using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<ShoppingList> InsertShoppingListAsync(
        ShoppingList shoppingList,
        CancellationToken cancellationToken);
}
