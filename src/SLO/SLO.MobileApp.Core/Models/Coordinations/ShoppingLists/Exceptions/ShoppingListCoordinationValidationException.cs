using System;

namespace SLO.MobileApp.Core.Models.Coordinations.ShoppingLists.Exceptions;

public class ShoppingListCoordinationValidationException : Exception
{
    public ShoppingListCoordinationValidationException(
        string exceptionMessage,
        Exception innerException)
        : base(exceptionMessage, innerException) { }
}
