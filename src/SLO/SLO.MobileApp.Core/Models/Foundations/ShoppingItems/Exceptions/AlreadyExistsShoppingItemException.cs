using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;

internal class AlreadyExistsShoppingItemException : Exception
{
    public AlreadyExistsShoppingItemException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
