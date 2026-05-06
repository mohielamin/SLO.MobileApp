using Xeptions;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;

public class InvalidShoppingListException : Xeption
{
    public InvalidShoppingListException(
        string exceptionMessage)
    : base(exceptionMessage) { }
}
