using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;

public class ShoppingListServiceException : Exception
{
    public ShoppingListServiceException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
