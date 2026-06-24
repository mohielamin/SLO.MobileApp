using Xeptions;

namespace SLO.MobileApp.Core.Models.Processings.ShoppingLists.Exceptions;

public class InvalidShoppingListProcessingException : Xeption
{
    public InvalidShoppingListProcessingException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
