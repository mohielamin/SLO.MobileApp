using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;

internal partial class ShoppingListItemService
{
    private void ValidateShoppingListItem(
        ShoppingListItem shoppingListItem)
    {
        if (shoppingListItem is null)
        {
            throw new NullShoppingListItemException(
                exceptionMessage: "Shopping list item is null.");
        }
    }
}
