using System;

namespace SLO.MobileApp.Core.Models.Processings.ShoppingLists.Exceptions;

public class FailedShoppingListProcessingServiceException : Exception
{
    public FailedShoppingListProcessingServiceException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
