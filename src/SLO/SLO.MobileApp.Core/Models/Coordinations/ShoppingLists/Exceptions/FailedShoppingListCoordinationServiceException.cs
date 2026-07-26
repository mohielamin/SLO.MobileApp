using System;

namespace SLO.MobileApp.Core.Models.Coordinations.ShoppingLists.Exceptions;

public class FailedShoppingListCoordinationServiceException : Exception
{
    public FailedShoppingListCoordinationServiceException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
