using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;

public class ShoppingListItemDependencyValidationException : Exception
{
    public ShoppingListItemDependencyValidationException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
