using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;

public class FailedShoppingListStorageException : Exception
{
    public FailedShoppingListStorageException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
