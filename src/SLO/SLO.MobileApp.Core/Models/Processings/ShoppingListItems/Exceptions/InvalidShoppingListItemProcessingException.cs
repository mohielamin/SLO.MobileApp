using Xeptions;

namespace SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;

public class InvalidShoppingListItemProcessingException : Xeption
{
    public InvalidShoppingListItemProcessingException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
