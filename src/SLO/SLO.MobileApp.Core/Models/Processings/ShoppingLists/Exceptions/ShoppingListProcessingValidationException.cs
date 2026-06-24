using System;

namespace SLO.MobileApp.Core.Models.Processings.ShoppingLists.Exceptions;

public class ShoppingListProcessingValidationException : Exception
{
    public ShoppingListProcessingValidationException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
