using System;

namespace SLO.MobileApp.Models.Foundations.ShoppingItems.Exceptions;

internal class ShoppingItemValidationException : Exception
{
    public ShoppingItemValidationException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
