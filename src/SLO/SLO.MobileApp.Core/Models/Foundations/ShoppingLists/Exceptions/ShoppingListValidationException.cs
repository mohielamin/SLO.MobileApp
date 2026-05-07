using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;

public class ShoppingListValidationException : Exception
{
    public ShoppingListValidationException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
