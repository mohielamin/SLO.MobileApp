using System;

namespace SLO.MobileApp.Core.Models.Coordinations.ShoppingLists.Exceptions;

public class ShoppingListCoordinationServiceException : Exception
{
    public ShoppingListCoordinationServiceException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
