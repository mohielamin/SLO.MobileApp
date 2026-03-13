using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;

internal class LockedShoppingItemException : Exception
{
    public LockedShoppingItemException(
        string exceptionMessage,
        Exception innerException)
         : base(exceptionMessage, innerException) { }
}
