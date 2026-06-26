using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;

namespace SLO.MobileApp.Core.Services.Processings.ShoppingListItems;

internal partial class ShoppingListItemProcessingService
{
    private void ValidateShoppingListItem(
        ShoppingListItem shoppingListItem)
    {
        if (shoppingListItem is null)
        {
            throw new NullShoppingListItemProcessingException(
                exceptionMessage: "Shopping list item is null.");
        }
    }
}
