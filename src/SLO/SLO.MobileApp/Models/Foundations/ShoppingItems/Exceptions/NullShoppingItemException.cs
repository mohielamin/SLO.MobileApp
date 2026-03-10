using System;

namespace SLO.MobileApp.Models.Foundations.ShoppingItems.Exceptions;

internal class NullShoppingItemException : Exception
{
    public NullShoppingItemException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
