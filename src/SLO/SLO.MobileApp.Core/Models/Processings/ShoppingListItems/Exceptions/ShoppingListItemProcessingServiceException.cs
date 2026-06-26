using System;

namespace SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;

public class ShoppingListItemProcessingServiceException : Exception
{
    public ShoppingListItemProcessingServiceException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
