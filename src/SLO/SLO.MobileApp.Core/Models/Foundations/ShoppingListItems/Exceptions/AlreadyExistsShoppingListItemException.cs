using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;

public class AlreadyExistsShoppingListItemException : Exception
{
    public AlreadyExistsShoppingListItemException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
