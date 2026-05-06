using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingLists;

internal partial class ShoppingListService
{
    private static void ValidateShoppingList(
        ShoppingList shoppingList)
    {
        if (shoppingList is null)
        {
            throw new NullShoppingListException(
                exceptionMessage: "Shopping list is null.");
        }
    }
}
