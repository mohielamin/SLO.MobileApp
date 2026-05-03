using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;

public class ShoppingListItemValidationException : Exception
{
    public ShoppingListItemValidationException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
