using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;

public class FailedShoppingListServiceException : Exception
{
    public FailedShoppingListServiceException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
