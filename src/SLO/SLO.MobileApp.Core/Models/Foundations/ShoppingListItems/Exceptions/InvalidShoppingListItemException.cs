using Xeptions;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;

public class InvalidShoppingListItemException : Xeption
{
    public InvalidShoppingListItemException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
