using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;

internal class ShoppingItemServiceException : Exception
{
    public ShoppingItemServiceException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
