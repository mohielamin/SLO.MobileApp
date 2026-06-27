using System;

namespace SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;

public class FailedShoppingListItemProcessingServiceException : Exception
{
    public FailedShoppingListItemProcessingServiceException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
