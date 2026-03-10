using SLO.MobileApp.Models.Foundations.ShoppingItems;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Services.Foundations.ShoppingItems;

internal interface IShoppingItemService
{
    ValueTask<ShoppingItem> AddShoppingItemAsync(
        ShoppingItem shoppingItem, CancellationToken cancellationToken);
}
