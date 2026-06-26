using System;

namespace SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;

public class ShoppingListItemProcessingDependencyValidationException : Exception
{
    public ShoppingListItemProcessingDependencyValidationException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
