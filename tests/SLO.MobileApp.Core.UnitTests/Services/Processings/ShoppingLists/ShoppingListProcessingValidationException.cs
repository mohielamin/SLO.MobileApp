using System;

namespace SLO.MobileApp.Core.UnitTests.Services.Processings.ShoppingLists;

public class ShoppingListProcessingValidationException : Exception
{
    public ShoppingListProcessingValidationException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
