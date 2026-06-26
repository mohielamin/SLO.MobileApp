using System;

namespace SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;

public class ShoppingListItemProcessingValidationException : Exception
{
    public ShoppingListItemProcessingValidationException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
