using Xeptions;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;

internal class InvalidShoppingItemException : Xeption
{
    public InvalidShoppingItemException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
