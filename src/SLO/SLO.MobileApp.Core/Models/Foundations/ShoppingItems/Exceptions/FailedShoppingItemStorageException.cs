using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;

internal class FailedShoppingItemStorageException : Exception
{
    public FailedShoppingItemStorageException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
