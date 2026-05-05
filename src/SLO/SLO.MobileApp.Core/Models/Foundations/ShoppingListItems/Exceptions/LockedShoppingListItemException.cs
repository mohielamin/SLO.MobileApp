using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;

public class LockedShoppingListItemException : Exception
{
    public LockedShoppingListItemException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
