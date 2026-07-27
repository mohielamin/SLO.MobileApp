using Xeptions;

namespace SLO.MobileApp.Core.Models.Coordinations.ShoppingLists.Exceptions;

public class InvalidShoppingListCoordinationException : Xeption
{
    public InvalidShoppingListCoordinationException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
