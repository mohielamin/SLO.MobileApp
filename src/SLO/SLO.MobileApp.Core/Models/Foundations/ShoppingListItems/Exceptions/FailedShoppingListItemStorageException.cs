using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;

public class FailedShoppingListItemStorageException : Exception
{
    public FailedShoppingListItemStorageException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
