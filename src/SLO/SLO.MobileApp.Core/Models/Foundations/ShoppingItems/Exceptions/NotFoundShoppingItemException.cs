using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;

internal class NotFoundShoppingItemException : Exception
{
    public NotFoundShoppingItemException(
        string exceptionMessage)
        : base(exceptionMessage) { }
}
