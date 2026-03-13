using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;

internal class NullShoppingItemException : Exception
{
    public NullShoppingItemException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
