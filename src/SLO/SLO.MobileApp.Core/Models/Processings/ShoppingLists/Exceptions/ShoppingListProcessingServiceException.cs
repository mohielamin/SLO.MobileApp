using System;

namespace SLO.MobileApp.Core.Models.Processings.ShoppingLists.Exceptions;

public class ShoppingListProcessingServiceException : Exception
{
    public ShoppingListProcessingServiceException(
        string exceptionMessage,
        Exception innerException)
    : base(exceptionMessage, innerException) { }
}
