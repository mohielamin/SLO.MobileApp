using SLO.MobileApp.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Models.Foundations.ShoppingItems.Exceptions;

namespace SLO.MobileApp.Services.Foundations.ShoppingItems;

internal sealed partial class ShoppingItemService
{
    private static void ValidateShoppingItem(
        ShoppingItem shoppingItem)
    {
        if (shoppingItem is null)
        {
            throw new NullShoppingItemException(
                exceptionMessage: "Shopping item is null.");
        }
    }
}
